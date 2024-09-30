using Application.Abstractions;
using Application.Exceptions;
using Application.Services;
using Domain.StronglyTypedIds;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace ModuleHouseAccountingApplication.Application.Tests.ServicesTests;

public class HousePostServiceTests
{
    private readonly IApplicationDbContext _context;
    private readonly IHousePostService _service;
    private readonly Mock<ILogger<HousePostService>> _mockLogger;

    public HousePostServiceTests()
    {
        var rawContext = TestHelper.GetTestDbContext();

        _context = rawContext;
        _mockLogger = new Mock<ILogger<HousePostService>>();

        _service = new HousePostService(_context, _mockLogger.Object);
    }
    
    [Fact]
    public async Task AddHousePostRelationsAsync_ShouldAddHousePosts_WhenPostIdsToAddIsNotEmpty()
    {
        // Arrange
        var houseId = new HouseId("house1");
        var postIdsToAdd = new List<PostId> { new PostId(1), new PostId(2) };

        // Act
        await _service.AddHousePostRelationsForNewHouseAsync(houseId, postIdsToAdd);
        await _context.SaveChangesAsync();

        // Assert
        var housePosts = await _context.HousePosts.Where(hp => hp.HouseId == houseId).ToListAsync();
        Assert.Equal(2, housePosts.Count);
        Assert.Contains(housePosts, hp => hp.PostId.Value == 1);
        Assert.Contains(housePosts, hp => hp.PostId.Value == 2);
    }
    
    [Fact]
    public async Task AddHousePostRelationsAsync_ShouldDoNothing_WhenPostIdsToAddIsEmpty()
    {
        // Arrange
        var houseId = new HouseId("house1");
        var postIdsToAdd = new List<PostId>();

        // Act
        await _service.AddHousePostRelationsForNewHouseAsync(houseId, postIdsToAdd);
        await _context.SaveChangesAsync();

        // Assert
        var housePosts = await _context.HousePosts.Where(hp => hp.HouseId == houseId).ToListAsync();
        Assert.Empty(housePosts);
    }
    
    [Fact]
    public async Task UpdatePostsForHouseAsync_ShouldThrowException_WhenHouseDoesNotExist()
    {
        // Arrange
        var houseId = new HouseId("nonexistent");
        var newPostIds = new List<PostId> { new PostId(1) };

        // Act & Assert
        var exception = await Assert.ThrowsAsync<EntityNotFoundException>(() =>
            _service.UpdatePostsForHouseAsync(houseId, newPostIds));

        Assert.Equal($"House with ID nonexistent not found", exception.Message);
        _mockLogger.Verify(logger => logger.Log(
            LogLevel.Warning, 
            It.IsAny<EventId>(), 
            It.IsAny<It.IsAnyType>(), 
            It.IsAny<EntityNotFoundException>(), 
            It.IsAny<Func<It.IsAnyType, Exception, string>>()!), Times.Once);
    }
    
    [Fact]
    public async Task UpdatePostsForHouseAsync_ShouldThrowException_WhenPostsDoNotExist()
    {
        // Arrange
        var newPostIds = new List<PostId> { new PostId(99) };
        var houseId = await _context.Houses.Select(x => x.Id).FirstAsync();

        // Act & Assert
        var exception = await Assert.ThrowsAsync<EntityNotFoundException>(() =>
            _service.UpdatePostsForHouseAsync(houseId, newPostIds));

        Assert.Contains("The following Post IDs do not exist: 99", exception.Message);
        _mockLogger.Verify(logger => logger.Log(
            LogLevel.Warning, 
            It.IsAny<EventId>(), 
            It.IsAny<It.IsAnyType>(), 
            It.IsAny<EntityNotFoundException>(), 
            It.IsAny<Func<It.IsAnyType, Exception, string>>()!), Times.Once);
    }
    
    [Fact]
    public async Task UpdatePostsForHouseAsync_ShouldAddAndRemoveHousePostsCorrectly()
    {
        // Arrange
        var house = await _context.Houses.FirstAsync();
        var newPostIds = new List<PostId> { new PostId(2), new PostId(3) };
        

        // Act
        await _service.UpdatePostsForHouseAsync(house.Id, newPostIds);
        await _context.SaveChangesAsync();

        // Assert
        var housePosts = await _context.HousePosts.Where(hp => hp.HouseId == house.Id).ToListAsync();
        Assert.Equal(2, housePosts.Count);
        Assert.Contains(housePosts, hp => hp.PostId.Value == 2);
        Assert.Contains(housePosts, hp => hp.PostId.Value == 3);
    }
}