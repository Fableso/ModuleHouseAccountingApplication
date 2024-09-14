using Application.Abstractions;
using Application.DTO.HouseWeekInfo.Response;
using Application.Exceptions;
using Application.Services;
using AutoMapper;
using Domain.Enums;
using Domain.StronglyTypedIds;
using Domain.ValueObjects;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ModuleHouseAccountingApplication.Application.Tests.ServicesTests;

public class HouseWeekInfoServiceTests
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IHouseWeekInfoService _service;
    public HouseWeekInfoServiceTests()
    {
        var options = new DbContextOptionsBuilder<MhDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        
        var rawContext = new MhDbContext(options);

        TestHelper.SeedData(rawContext);
        
        _context = rawContext;
        
        _mapper = TestHelper.CreateMapperProfile();
        _service = new HouseWeekInfoService(_context, _mapper);
    }
    
    [Fact]
    public async Task GetByIdAsync_HouseWeekInfoExists_ShouldReturnHouseWeekInfoResponseWithCorrectMapping()
    {
        // Arrange
        var houseWeekInfoId = (await _context.HouseWeekInfos.FirstAsync())!.Id;
        var expectedHouseWeekInfoResponse = TestHelper.ExpectedHouseWeekInfoResponses().First();
        expectedHouseWeekInfoResponse.Id = houseWeekInfoId;
        
        // Act
        var houseWeekInfoResponse = await _service.GetByIdAsync(houseWeekInfoId);
        
        // Assert
        Assert.NotNull(houseWeekInfoResponse);
        Assert.True(new HouseWeekInfoResponseComparer().Equals(expectedHouseWeekInfoResponse, houseWeekInfoResponse));
    }
    
    [Fact]
    public async Task GetByIdAsync_HouseWeekInfoDoesNotExist_ShouldReturnNull()
    {
        // Arrange
        var nonExistentHouseWeekInfoId = new HouseWeekInfoId(999);
        
        // Act
        var houseWeekInfoResponse = await _service.GetByIdAsync(nonExistentHouseWeekInfoId);
        
        // Assert
        Assert.Null(houseWeekInfoResponse);
    }
    
    [Fact]
    public async Task GetHouseInfosInTimeSpanAsync_HouseWeekInfosExist_ShouldReturnHouseWeekInfoResponsesWithCorrectMapping()
    {
        // Arrange
        var dateSpan = DateSpan.Create(new DateOnly(2022, 1, 1), new DateOnly(2024, 1, 7)).Value;
        
        // Act
        var houseWeekInfoResponses = (await _service.GetHouseInfosInTimeSpanAsync(dateSpan)).ToList();
        
        // Assert
        Assert.NotNull(houseWeekInfoResponses);
        Assert.NotEmpty(houseWeekInfoResponses);
        Assert.All(houseWeekInfoResponses, hwir =>
            {
                Assert.InRange(hwir.StartDate, dateSpan.StartDate, dateSpan.EndDate ?? DateOnly.MaxValue);
            }
        );
    }
    
    [Fact]
    public async Task GetHouseInfosInTimeSpanAsync_NoHouseWeekInfosExist_ShouldReturnEmptyList()
    {
        // Arrange
        var dateSpan = DateSpan.Create(new DateOnly(2026, 1, 1), new DateOnly(2026, 12, 31)).Value;
        
        // Act
        var houseWeekInfoResponses = await _service.GetHouseInfosInTimeSpanAsync(dateSpan);
        
        // Assert
        Assert.Empty(houseWeekInfoResponses);
    }
    
    [Fact]
    public async Task GetHouseInfosForHouseAsync_HouseExists_ShouldReturnHouseWeekInfoResponsesWithCorrectMapping()
    {
        // Arrange
        var houseId = new HouseId("MB 56-1");
        
        // Act
        var houseWeekInfoResponses = (await _service.GetHouseInfosForHouseAsync(houseId)).ToList();
        
        // Assert
        Assert.NotNull(houseWeekInfoResponses);
        Assert.NotEmpty(houseWeekInfoResponses);
        Assert.Equal(3, houseWeekInfoResponses.Count);
    }
    
    [Fact]
    public async Task GetHouseInfosForHouseAsync_HouseDoesNotExist_ShouldThrowEntityNotFoundException()
    {
        // Arrange
        var nonExistentHouseId = new HouseId("nonexistent");
        
        // Act & Assert
        var exception = await Assert.ThrowsAsync<EntityNotFoundException>(() => _service.GetHouseInfosForHouseAsync(nonExistentHouseId));
        Assert.Equal($"House with ID nonexistent not found", exception.Message);
    }
    
    [Fact]
    public async Task AddAsync_ShouldAddHouseWeekInfoToDatabase()
    {
        // Arrange
        var createHouseWeekInfoRequest = TestHelper.GetCreateHouseWeekInfoRequest();
        var expectedHouseWeekInfoAmount = await _context.HouseWeekInfos.CountAsync() + 1;
        
        // Act
        var houseWeekInfoResponse = await _service.AddAsync(createHouseWeekInfoRequest);
        
        // Assert
        Assert.NotNull(houseWeekInfoResponse);
        Assert.Equal(expectedHouseWeekInfoAmount, await _context.HouseWeekInfos.CountAsync());
        Assert.Equal(createHouseWeekInfoRequest.HouseId, houseWeekInfoResponse.HouseId);
        Assert.Equal(createHouseWeekInfoRequest.StartDate, houseWeekInfoResponse.StartDate);
        Assert.Equal(createHouseWeekInfoRequest.Status, houseWeekInfoResponse.Status);
        Assert.Equal(createHouseWeekInfoRequest.OnTime, houseWeekInfoResponse.OnTime);
    }
    
    [Fact]
    public async Task UpdateAsync_HouseWeekInfoExists_ShouldUpdateHouseWeekInfoInDatabase()
    {
        // Arrange
        var houseWeekInfoId = (await _context.HouseWeekInfos.FirstAsync())!.Id;
        var updateHouseWeekInfoRequest = TestHelper.GetUpdateHouseWeekInfoRequest();
        updateHouseWeekInfoRequest.Id = houseWeekInfoId;
        var expectedHouseWeekInfoAmount = await _context.HouseWeekInfos.CountAsync();
        
        // Act
        var houseWeekInfoResponse = await _service.UpdateStatusAsync(updateHouseWeekInfoRequest);
        
        // Assert
        Assert.NotNull(houseWeekInfoResponse);
        Assert.Equal(expectedHouseWeekInfoAmount, await _context.HouseWeekInfos.CountAsync());
        Assert.Equal(updateHouseWeekInfoRequest.Id, houseWeekInfoResponse.Id);
        Assert.Equal(updateHouseWeekInfoRequest.HouseId, houseWeekInfoResponse.HouseId);
        Assert.Equal(updateHouseWeekInfoRequest.Status, houseWeekInfoResponse.Status);
        Assert.Equal(updateHouseWeekInfoRequest.OnTime, houseWeekInfoResponse.OnTime);
    }
    
    [Fact]
    public async Task UpdateAsync_HouseWeekInfoDoesNotExist_ShouldThrowEntityNotFoundException()
    {
        // Arrange
        var updateHouseWeekInfoRequest = TestHelper.GetUpdateHouseWeekInfoRequest();
        updateHouseWeekInfoRequest.Id = new HouseWeekInfoId(999);
        
        // Act & Assert
        var exception = await Assert.ThrowsAsync<EntityNotFoundException>(() => _service.UpdateStatusAsync(updateHouseWeekInfoRequest));
        Assert.Equal($"HouseWeekInfo with ID 999 not found", exception.Message);
    }
    
    [Fact]
    public async Task RemoveByIdAsync_HouseWeekInfoExists_ShouldRemoveHouseWeekInfoFromDatabase()
    {
        // Arrange
        var houseWeekInfoId = (await _context.HouseWeekInfos.FirstAsync())!.Id;
        var expectedHouseWeekInfoAmount = await _context.HouseWeekInfos.CountAsync() - 1;
        
        // Act
        await _service.RemoveByIdAsync(houseWeekInfoId);
        var houseWeekInfo = await _context.HouseWeekInfos.FindAsync(houseWeekInfoId);
        
        // Assert
        Assert.Null(houseWeekInfo);
        Assert.Equal(expectedHouseWeekInfoAmount, await _context.HouseWeekInfos.CountAsync());
    }
    
    [Fact]
    public async Task RemoveByIdAsync_HouseWeekInfoDoesNotExist_ShouldThrowEntityNotFoundException()
    {
        // Arrange
        var nonExistentHouseWeekInfoId = new HouseWeekInfoId(999);
        
        // Act & Assert
        var exception = await Assert.ThrowsAsync<EntityNotFoundException>(() => _service.RemoveByIdAsync(nonExistentHouseWeekInfoId));
        Assert.Equal($"HouseWeekInfo with ID 999 not found", exception.Message);
    }
}