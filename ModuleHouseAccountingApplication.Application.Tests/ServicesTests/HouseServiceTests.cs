using Application.Abstractions;
using Application.Exceptions;
using Application.Services;
using Domain.Enums;
using Domain.StronglyTypedIds;
using Domain.ValueObjects;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace ModuleHouseAccountingApplication.Application.Tests.ServicesTests;

public class HouseServiceTests
{
    private readonly IApplicationDbContext _context;
    private readonly Mock<IHousePostService> _mockHousePostService;
    private readonly Mock<ILogger<HouseService>> _mockLogger;
    private readonly IHouseService _service;

    public HouseServiceTests()
    {
        var rawContext = TestHelper.GetTestDbContext();
        
        _context = rawContext;
        _mockLogger = new Mock<ILogger<HouseService>>();
        var mapper = TestHelper.CreateMapperProfile();
        _mockHousePostService = new Mock<IHousePostService>();

        _service = new HouseService(_context, mapper, _mockHousePostService.Object, _mockLogger.Object);
    }
    
    [Fact]
    public async Task GetByIdAsync_HouseExists_ShouldReturnHouseResponseWithCorrectMapping()
    {
        // Arrange
        var houseId = new HouseId("MB 110-1");
        var expectedHouseResponse = TestHelper.ExpectedHouseResponses().First();
        
        // Act
        var houseResponse = await _service.GetByIdAsync(houseId);
        
        // Assert
        Assert.NotNull(houseResponse);
        Assert.True(new HouseResponseComparer().Equals(expectedHouseResponse, houseResponse));
        Assert.Equal(2, houseResponse.Posts.Count);
        Assert.Equal(3, houseResponse.HouseWeekInfos.Count);
        Assert.Equal(5, houseResponse.HouseWeekInfos.SelectMany(x => x.WeekMarkResponses).Count());
    }
    
    [Fact]
    public async Task GetByIdAsync_HouseDoesNotExist_ShouldReturnNull()
    {
        // Arrange
        var nonExistentHouseId = new HouseId("NON_EXISTENT_ID");

        // Act
        var houseResponse = await _service.GetByIdAsync(nonExistentHouseId);

        // Assert
        Assert.Null(houseResponse);
    }
    
    [Fact]
    public async Task GetHousesInDateRangeAsync_HousesExistInRange_ShouldReturnHouseResponses()
    {
        // Arrange
        var dateSpan = DateSpan.Create(new DateOnly(2023, 9, 1), new DateOnly(2024, 12, 31)).Value;

        // Act
        var houseResponses = (await _service.GetHousesInDateRangeAsync(dateSpan)).ToList();

        // Assert
        Assert.NotNull(houseResponses);
        Assert.NotEmpty(houseResponses);
        Assert.All(houseResponses, house =>
        {
            Assert.InRange(house.OfficialStartDate, dateSpan.StartDate, dateSpan.EndDate ?? DateOnly.MaxValue);
            Assert.InRange(house.OfficialEndDate ?? dateSpan.EndDate ?? DateOnly.MaxValue, dateSpan.StartDate, dateSpan.EndDate ?? DateOnly.MaxValue);
        });
    }
    
    [Fact]
    public async Task GetHousesInDateRangeAsync_NoHousesInRange_ShouldReturnEmptyList()
    {
        // Arrange
        var dateSpan = DateSpan.Create(new DateOnly(2026, 1, 1), new DateOnly(2026, 12, 31)).Value;

        // Act
        var houseResponses = await _service.GetHousesInDateRangeAsync(dateSpan);

        // Assert
        Assert.NotNull(houseResponses);
        Assert.Empty(houseResponses);
    }
    
    [Fact]
    public async Task GetHousesByStateAsync_HousesExistWithState_ShouldReturnHouseResponses()
    {
        // Arrange
        var state = HouseStatus.InProcess;

        // Act
        var houseResponses = (await _service.GetHousesByStateAsync(state)).ToList();

        // Assert
        Assert.NotNull(houseResponses);
        Assert.NotEmpty(houseResponses);
        Assert.Equal(2, houseResponses.Count);
        Assert.All(houseResponses, house => Assert.Equal(state, house.CurrentState));
        Assert.All(houseResponses,
            house => Assert.True(new HouseResponseComparer()
                .Equals(TestHelper.ExpectedHouseResponses().Single(x => x.Model == house.Model), house)));
    }
    
    [Fact]
    public async Task GetHousesByStateAsync_NoHousesWithState_ShouldReturnEmptyList()
    {
        // Arrange
        var state = HouseStatus.Finished;

        // Act
        var houseResponses = await _service.GetHousesByStateAsync(state);

        // Assert
        Assert.NotNull(houseResponses);
        Assert.Empty(houseResponses);
    }
    
    [Fact]
    public async Task AddAsync_ValidHouse_ShouldAddHouseAndReturnResponse()
    {
        // Arrange
        var createRequest = TestHelper.GetCreateHouseRequest();
        var expectedHouseResponseAmount = await _context.Houses.CountAsync() + 1;

        // Act
        var houseResponse = await _service.AddAsync(createRequest);

        // Assert
        Assert.NotNull(houseResponse);
        Assert.Equal(expectedHouseResponseAmount, await _context.Houses.CountAsync());
        Assert.Equal(createRequest.Model, houseResponse.Model);
        Assert.Equal(createRequest.Length, houseResponse.Length);
        Assert.Equal(createRequest.Width, houseResponse.Width);
        Assert.Equal(createRequest.TopLeftCornerX, houseResponse.TopLeftCornerX);
        Assert.Equal(createRequest.TopLeftCornerY, houseResponse.TopLeftCornerY);
        Assert.Equal(createRequest.OfficialStartDate, houseResponse.OfficialStartDate);
        Assert.Equal(createRequest.OfficialEndDate, houseResponse.OfficialEndDate);
        Assert.Equal(createRequest.Brigade, houseResponse.Brigade);
        Assert.Equal(HouseStatus.Planned, houseResponse.CurrentState);
        _mockHousePostService.Verify(
            service => service.AddHousePostRelationsForNewHouseAsync(createRequest.Model, createRequest.PostIds, It.IsAny<CancellationToken>()),
            Times.Once);
    }
    
    [Fact]
    public async Task AddAsync_HouseModelAlreadyExists_ShouldThrowEntityAlreadyExistsException()
    {
        // Arrange
        var createRequest = TestHelper.GetCreateHouseRequest();
        createRequest.Model = new HouseId("MB 110-1");

        // Act & Assert
        var exception = await Assert.ThrowsAsync<EntityAlreadyExistsException>(() => _service.AddAsync(createRequest));
        Assert.Equal("House with model MB 110-1 already exists. The house model must be unique", exception.Message);
        _mockLogger.Verify(logger => logger.Log(
            LogLevel.Warning, 
            It.IsAny<EventId>(), 
            It.IsAny<It.IsAnyType>(), 
            It.IsAny<EntityAlreadyExistsException>(), 
            It.IsAny<Func<It.IsAnyType, Exception, string>>()!), Times.Once);
    }
    
    [Fact]
    public async Task UpdateAsync_HouseExists_ShouldUpdateHouseAndReturnResponse()
    {
        // Arrange
        var updateRequest = TestHelper.GetUpdateHouseRequest();

        // Act
        var houseResponse = await _service.UpdateAsync(updateRequest);

        // Assert
        Assert.NotNull(houseResponse);
        Assert.Equal(updateRequest.Model, houseResponse.Model);
        Assert.Equal(updateRequest.Length, houseResponse.Length);
        Assert.Equal(updateRequest.Width, houseResponse.Width);
        Assert.Equal(updateRequest.TopLeftCornerX, houseResponse.TopLeftCornerX);
        Assert.Equal(updateRequest.TopLeftCornerY, houseResponse.TopLeftCornerY);
        Assert.Equal(updateRequest.OfficialStartDate, houseResponse.OfficialStartDate);
        Assert.Equal(updateRequest.OfficialEndDate, houseResponse.OfficialEndDate);
        Assert.Equal(updateRequest.Brigade, houseResponse.Brigade);
        Assert.Equal(HouseStatus.Planned, houseResponse.CurrentState);
        _mockHousePostService.Verify(
            service => service.UpdatePostsForHouseAsync(updateRequest.Model, updateRequest.PostIds, It.IsAny<CancellationToken>()),
            Times.Once);
    }
    
    [Fact]
    public async Task UpdateAsync_HouseDoesNotExist_ShouldThrowEntityNotFoundException()
    {
        // Arrange
        var updateRequest = TestHelper.GetUpdateHouseRequest();
        updateRequest.Model = new HouseId("NON_EXISTENT_ID");

        // Act & Assert
        var exception = await Assert.ThrowsAsync<EntityNotFoundException>(() => _service.UpdateAsync(updateRequest));
        Assert.Equal("House with ID NON_EXISTENT_ID not found", exception.Message);
        _mockLogger.Verify(logger => logger.Log(
            LogLevel.Warning, 
            It.IsAny<EventId>(), 
            It.IsAny<It.IsAnyType>(), 
            It.IsAny<EntityNotFoundException>(), 
            It.IsAny<Func<It.IsAnyType, Exception, string>>()!), Times.Once);
    }
    
    [Fact]
    public async Task RemoveByIdAsync_HouseExists_ShouldRemoveHouse()
    {
        // Arrange
        var houseId = new HouseId("MB 110-1");
        var expectedHouseResponseAmount = await _context.Houses.CountAsync() - 1;

        // Act
        await _service.RemoveByIdAsync(houseId);

        // Assert
        Assert.Equal(expectedHouseResponseAmount, await _context.Houses.CountAsync());
        Assert.Null(await _context.Houses.FindAsync(houseId));
    }
    
    [Fact]
    public async Task RemoveByIdAsync_HouseDoesNotExist_ShouldThrowEntityNotFoundException()
    {
        // Arrange
        var nonExistentHouseId = new HouseId("NON_EXISTENT_ID");

        // Act & Assert
        var exception = await Assert.ThrowsAsync<EntityNotFoundException>(() => _service.RemoveByIdAsync(nonExistentHouseId));
        Assert.Equal("House with ID NON_EXISTENT_ID not found", exception.Message);
        _mockLogger.Verify(logger => logger.Log(
            LogLevel.Warning, 
            It.IsAny<EventId>(), 
            It.IsAny<It.IsAnyType>(), 
            It.IsAny<EntityNotFoundException>(), 
            It.IsAny<Func<It.IsAnyType, Exception, string>>()!), Times.Once);
    }
    
    [Fact]
    public async Task RemoveByIdAsync_HouseHasRelations_ShouldRemoveHouseAndRelations()
    {
        // Arrange
        var houseId = new HouseId("MB 140-1");
        var expectedHouseResponseAmount = await _context.Houses.CountAsync() - 1;

        // Act
        await _service.RemoveByIdAsync(houseId);

        // Assert
        Assert.Equal(expectedHouseResponseAmount, await _context.Houses.CountAsync());
        Assert.Null(await _context.Houses.FindAsync(houseId));
        Assert.Empty(await _context.HousePosts.Where(hp => hp.HouseId == houseId).ToListAsync());
        Assert.Empty(await _context.HouseWeekInfos.Where(hwi => hwi.HouseId == houseId).ToListAsync());
        Assert.Empty(await _context.WeekMarks.Where(wm => wm.HouseWeekInfo.HouseId == houseId).ToListAsync());
    }
    
}