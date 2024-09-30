using Application.Abstractions;
using Application.DTO.WeekMark.Response;
using Application.Exceptions;
using Application.Services;
using Domain.StronglyTypedIds;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace ModuleHouseAccountingApplication.Application.Tests.ServicesTests;

public class WeekMarkServiceTests
{
    private readonly IApplicationDbContext _context;
    private readonly IWeekMarkService _service;
    public WeekMarkServiceTests()
    {
        var rawContext = TestHelper.GetTestDbContext();
        
        _context = rawContext;
        Mock<ILogger<WeekMarkService>> mockLogger = new();
        var mapper = TestHelper.CreateMapperProfile();
        _service = new WeekMarkService(_context, mapper, mockLogger.Object);
    }
    
    [Fact]
    public async Task GetByIdAsync_WeekMarkExists_ShouldReturnWeekMarkResponseWithCorrectMapping()
    {
        // Arrange
        var weekMark = await _context.WeekMarks.FirstAsync();
        var expectedWeekMarkResponse = new WeekMarkResponse
        {
            Id = weekMark.Id,
            HouseWeekInfoId = weekMark.HouseWeekInfoId,
            Comment = weekMark.Comment,
            MarkType= weekMark.MarkType
        };
        
        // Act
        var weekMarkResponse = await _service.GetByIdAsync(weekMark.Id);
        
        // Assert
        Assert.NotNull(weekMarkResponse);
        Assert.True(new WeekMarkResponseComparer().Equals(expectedWeekMarkResponse, weekMarkResponse));
    }
    
    [Fact]
    public async Task GetByIdAsync_WeekMarkDoesNotExist_ShouldReturnNull()
    {
        // Arrange
        var nonExistentWeekMarkId = new WeekMarkId(Guid.NewGuid());
        
        // Act
        var weekMarkResponse = await _service.GetByIdAsync(nonExistentWeekMarkId);
        
        // Assert
        Assert.Null(weekMarkResponse);
    }
    
    [Fact]
    public async Task AddAsync_ShouldAddWeekMarkToDatabase()
    {
        // Arrange
        var weekMarkRequest = TestHelper.GetCreateWeekMarkRequest();
        var houseWeekInfo = await _context.HouseWeekInfos.FirstAsync();
        weekMarkRequest.HouseWeekInfoId = houseWeekInfo.Id;
        var expectedWeekMarksAmount = await _context.WeekMarks.CountAsync() + 1;
        
        // Act
        var weekMarkResponse = await _service.AddAsync(weekMarkRequest);
        
        // Assert
        Assert.NotNull(weekMarkResponse);
        Assert.Equal(expectedWeekMarksAmount, await _context.WeekMarks.CountAsync());
        Assert.Equal(weekMarkRequest.Comment, weekMarkResponse.Comment);
        Assert.Equal(weekMarkRequest.MarkType, weekMarkResponse.MarkType);
    }
    
    [Fact]
    public async Task AddAsync_HouseWeekInfoDoesNotExist_ShouldThrowEntityNotFoundException()
    {
        // Arrange
        var weekMarkRequest = TestHelper.GetCreateWeekMarkRequest();
        weekMarkRequest.HouseWeekInfoId = new HouseWeekInfoId(long.MaxValue);
        
        // Act & Assert
        var exception = await Assert.ThrowsAsync<EntityNotFoundException>(() => _service.AddAsync(weekMarkRequest));
        Assert.Equal($"HouseWeekInfo with ID {weekMarkRequest.HouseWeekInfoId.Value} not found", exception.Message);
    }
    
    [Fact]
    public async Task RemoveByIdAsync_WeekMarkExists_ShouldRemoveWeekMarkFromDatabase()
    {
        // Arrange
        var weekMark = await _context.WeekMarks.FirstAsync();
        var expectedWeekMarksAmount = await _context.WeekMarks.CountAsync() - 1;
        
        // Act
        await _service.RemoveByIdAsync(weekMark.Id);
        
        // Assert
        Assert.Equal(expectedWeekMarksAmount, await _context.WeekMarks.CountAsync());
        Assert.Null(await _context.WeekMarks.FindAsync(weekMark.Id));
    }
    
    [Fact]
    public async Task RemoveByIdAsync_WeekMarkDoesNotExist_ShouldThrowEntityNotFoundException()
    {
        // Arrange
        var nonExistentWeekMarkId = new WeekMarkId(Guid.NewGuid());
        
        // Act & Assert
        var exception = await Assert.ThrowsAsync<EntityNotFoundException>(() => _service.RemoveByIdAsync(nonExistentWeekMarkId));
        Assert.Equal($"WeekMark with ID {nonExistentWeekMarkId.Value} not found", exception.Message);
    }
    
    [Fact]
    public async Task UpdateAsync_WeekMarkExists_ShouldUpdateWeekMarkInDatabase()
    {
        // Arrange
        var weekMark = await _context.WeekMarks.FirstAsync();
        var updateWeekMarkRequest = TestHelper.GetUpdateWeekMarkRequest();
        updateWeekMarkRequest.Id = weekMark.Id;
        var expectedWeekMarksAmount = await _context.WeekMarks.CountAsync();
        
        // Act
        var weekMarkResponse = await _service.UpdateAsync(updateWeekMarkRequest);
        
        // Assert
        Assert.NotNull(weekMarkResponse);
        Assert.Equal(expectedWeekMarksAmount, await _context.WeekMarks.CountAsync());
        Assert.Equal(updateWeekMarkRequest.Comment, weekMarkResponse.Comment);
        Assert.Equal(updateWeekMarkRequest.MarkType, weekMarkResponse.MarkType);
    }
    
    [Fact]
    public async Task UpdateAsync_WeekMarkDoesNotExist_ShouldThrowEntityNotFoundException()
    {
        // Arrange
        var updateWeekMarkRequest = TestHelper.GetUpdateWeekMarkRequest();
        updateWeekMarkRequest.Id = new WeekMarkId(Guid.NewGuid());
        
        // Act & Assert
        var exception = await Assert.ThrowsAsync<EntityNotFoundException>(() => _service.UpdateAsync(updateWeekMarkRequest));
        Assert.Equal($"WeekMark with ID {updateWeekMarkRequest.Id.Value} not found", exception.Message);
    }
    
    [Fact]
    public async Task GetWeekMarksByWeekInfoId_WeekMarksExist_ShouldReturnWeekMarkResponses()
    {
        // Arrange
        var houseWeekInfoId = await _context.HouseWeekInfos.Include(hwi => hwi.WeekComments).FirstAsync();
        var expectedWeekMarkResponsesAmount = houseWeekInfoId.WeekComments.Count;
        
        // Act
        var weekMarkResponses = await _service.GetWeekMarksByWeekInfoId(houseWeekInfoId.Id);
        
        // Assert
        Assert.NotNull(weekMarkResponses);
        Assert.Equal(expectedWeekMarkResponsesAmount, weekMarkResponses.Count());
    }
    
    [Fact]
    public async Task GetWeekMarksByWeekInfoId_WeekMarksDoNotExist_ShouldReturnEmptyCollection()
    {
        // Arrange
        var nonExistentHouseWeekInfoId = new HouseWeekInfoId(long.MaxValue);
        
        // Act
        var weekMarkResponses = await _service.GetWeekMarksByWeekInfoId(nonExistentHouseWeekInfoId);
        
        // Assert
        Assert.NotNull(weekMarkResponses);
        Assert.Empty(weekMarkResponses);
    }
}