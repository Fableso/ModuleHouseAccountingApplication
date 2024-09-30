using System.Security.Claims;
using Domain.Entities;
using Domain.Enums;
using Domain.StronglyTypedIds;
using Domain.ValueObjects;
using Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace ModuleHouseAccountingApplication.Infrastructure.Tests;

public class AuditSystemTests
{
    private readonly ServiceProvider _serviceProvider;

    public AuditSystemTests()
    {
        var services = new ServiceCollection();

        var httpContextAccessor = GetMockHttpContextAccessor();
        services.AddSingleton(httpContextAccessor);
        
        services.AddDbContext<MhDbContext>(options =>
        {
            options.UseInMemoryDatabase(Guid.NewGuid().ToString())
                .ConfigureWarnings(warnings => warnings.Ignore(InMemoryEventId.TransactionIgnoredWarning));
        });

        _serviceProvider = services.BuildServiceProvider();
    }

    private static IHttpContextAccessor GetMockHttpContextAccessor()
    {
        var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();

        var mockHttpContext = new DefaultHttpContext();

        mockHttpContext.Request.Headers["Authorization"] = "Bearer some-valid-token";

        var claims = new List<Claim>
        {
            new Claim("UserId", "TestUserId")
        };
        var identity = new ClaimsIdentity(claims, "TestAuthType");
        var principal = new ClaimsPrincipal(identity);
        mockHttpContext.User = principal;

        mockHttpContextAccessor.Setup(a => a.HttpContext).Returns(mockHttpContext);

        return mockHttpContextAccessor.Object;
    }
    
    private MhDbContext CreateDbContext()
    {
        return _serviceProvider.GetRequiredService<MhDbContext>();
    }
    
    private static House CreateTestHouse()
    {
        return new House(new HouseId("Test_house-1"),
            HouseMetrics.Create(5.5, 9.8).Value,
            new Point(-5, 5),
            HouseStatus.Planned,
            DateSpan.Create(new DateOnly(2024, 5, 17), new DateOnly(2025, 1, 6)).Value, Brigade.Create("TestBrigade").Value);
    }
    
    [Fact]
    public async Task AddEntity_ShouldCreateAuditRecord()
    {
        // Arrange
        await using var context = CreateDbContext();
        var newHouse = CreateTestHouse();

        // Act
        context.Houses.Add(newHouse);
        await context.SaveChangesAsync();

        // Assert
        var audits = await context.Audits.Include(a => a.Changes).ToListAsync();
        Assert.Single(audits);
        Assert.Equal("Create", audits[0].Operation);
        Assert.Equal(newHouse.GetType().Name, audits[0].TableName);
        Assert.Empty(audits[0].Changes);
    }
    
    [Fact]
    public async Task UpdateEntity_SinglePropertyUpdate_ShouldCreateAuditRecord()
    {
        // Arrange
        await using var context = CreateDbContext();
        var existingHouse = CreateTestHouse();
        context.Houses.Add(existingHouse);
        await context.SaveChangesAsync();

        // Act
        existingHouse.ChangeBrigade(Brigade.Create("UpdatedBrigade").Value);
        await context.SaveChangesAsync();

        // Assert
        var audits = await context.Audits.Include(a => a.Changes).ToListAsync();
        Assert.Equal(2, audits.Count);
        var updateAudit = audits[^1];
        Assert.Equal("Update", updateAudit.Operation);
        Assert.Contains(updateAudit.Changes,
            c => c is { FieldName: "Brigade", OldValue: "TestBrigade", NewValue: "UpdatedBrigade" });
    }
    
    [Fact]
    public async Task UpdateEntity_MultiplePropertiesUpdate_ShouldCreateAuditRecordAndMultipleAuditEntryRecords()
    {
        // Arrange
        await using var context = CreateDbContext();
        var existingHouse = CreateTestHouse();
        context.Houses.Add(existingHouse);
        await context.SaveChangesAsync();

        // Act
        existingHouse.ChangeHousePosition(new Point(10, 15));
        existingHouse.ChangeBrigade(Brigade.Create("UpdatedBrigade").Value);
        await context.SaveChangesAsync();

        // Assert
        var audits = await context.Audits.Include(a => a.Changes).ToListAsync();
        Assert.Equal(2, audits.Count);
        var updateAudit = audits[^1];
        Assert.Equal("Update", updateAudit.Operation);
        Assert.Contains(updateAudit.Changes,
            c => c is { FieldName: "TopLeftCornerX", OldValue: "-5", NewValue: "10" });
        Assert.Contains(updateAudit.Changes,
            c => c is { FieldName: "TopLeftCornerY", OldValue: "5", NewValue: "15" });
        Assert.Contains(updateAudit.Changes,
            c => c is { FieldName: "Brigade", OldValue: "TestBrigade", NewValue: "UpdatedBrigade" });
    }
    
    [Fact]
    public async Task DeleteEntity_ShouldCreateDeleteAuditRecord()
    {
        // Arrange
        await using var context = CreateDbContext();
        var house = CreateTestHouse();
        context.Houses.Add(house);
        await context.SaveChangesAsync();

        // Act
        context.Remove(house);
        await context.SaveChangesAsync();

        // Assert
        var audits = await context.Audits.Include(a => a.Changes).ToListAsync();
        Assert.Equal(2, audits.Count);
        var deleteAudit = audits[^1];
        Assert.Equal("Delete", deleteAudit.Operation);
        Assert.Empty(deleteAudit.Changes);
    }
}