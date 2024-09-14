using Application.Abstractions;
using Application.DTO.Post.Request;
using Application.Exceptions;
using Application.Services;
using AutoMapper;
using Domain.StronglyTypedIds;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace ModuleHouseAccountingApplication.Application.Tests.ServicesTests;

public class PostServiceTests
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IPostService _service;

    public PostServiceTests()
    {
        var options = new DbContextOptionsBuilder<MhDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        
        var rawContext = new MhDbContext(options);

        TestHelper.SeedData(rawContext);
        
        _context = rawContext;
        
        _mapper = TestHelper.CreateMapperProfile();
        _service = new PostService(_context, _mapper);
    }
    
    [Fact]
    public async Task GetByIdAsync_PostExists_ShouldReturnPostResponseWithCorrectMapping()
    {
        // Arrange
        var postId = new PostId(1);
        var expectedPostResponse = TestHelper.ExpectedPostResponses().First(x => x.Id == postId);
        
        // Act
        var postResponse = await _service.GetByIdAsync(postId);
        
        // Assert
        Assert.NotNull(postResponse);
        Assert.True(new PostResponseComparer().Equals(expectedPostResponse, postResponse));
    }
    
    [Fact]
    public async Task GetByIdAsync_PostDoesNotExist_ShouldReturnNull()
    {
        // Arrange
        var nonExistentPostId = new PostId(999);
        
        // Act
        var postResponse = await _service.GetByIdAsync(nonExistentPostId);
        
        // Assert
        Assert.Null(postResponse);
    }
    
    [Fact]
    public async Task AddAsync_ShouldAddPostToDatabase()
    {
        // Arrange
        var postRequest = TestHelper.GetCreatePostRequest();
        var expectedPostResponseAmount = await _context.Posts.CountAsync() + 1;
        
        // Act
        var postResponse = await _service.AddAsync(postRequest);
        
        // Assert
        Assert.NotNull(postResponse);
        Assert.Equal(expectedPostResponseAmount, await _context.Posts.CountAsync());
        Assert.Equal(postRequest.Name, postResponse.Name);
        Assert.Equal(postRequest.Area, postResponse.Area);
    }
    
    [Fact]
    public async Task UpdateAsync_PostExists_ShouldUpdatePostInDatabase()
    {
        // Arrange
        var postRequest = TestHelper.GetUpdatePostRequest();
        var expectedPostResponseAmount = await _context.Posts.CountAsync();
        
        // Act
        var postResponse = await _service.UpdateAsync(postRequest);
        
        // Assert
        Assert.NotNull(postResponse);
        Assert.Equal(expectedPostResponseAmount, await _context.Posts.CountAsync());
        Assert.Equal(postRequest.Name, postResponse.Name);
        Assert.Equal(postRequest.Area, postResponse.Area);
    }
    
    [Fact]
    public async Task UpdateAsync_PostDoesNotExist_ShouldThrowException()
    {
        // Arrange
        var postRequest = TestHelper.GetUpdatePostRequest();
        postRequest.Id = new PostId(999);
        
        // Act & Assert
        var exception = await Assert.ThrowsAsync<EntityNotFoundException>(() => _service.UpdateAsync(postRequest));
        Assert.Equal($"Post with ID 999 not found", exception.Message);
    }
    
    [Fact]
    public async Task RemoveByIdAsync_PostExists_ShouldRemovePostFromDatabase()
    {
        // Arrange
        var postId = new PostId(1);
        var expectedPostResponseAmount = await _context.Posts.CountAsync() - 1;
        
        // Act
        await _service.RemoveByIdAsync(postId);
        var post = await _context.Posts.FindAsync(postId);
        
        // Assert
        Assert.Null(post);
        Assert.Equal(expectedPostResponseAmount, await _context.Posts.CountAsync());
    }
    
    [Fact]
    public async Task RemoveByIdAsync_PostDoesNotExist_ShouldThrowException()
    {
        // Arrange
        var nonExistentPostId = new PostId(999);
        
        // Act & Assert
        var exception = await Assert.ThrowsAsync<EntityNotFoundException>(() => _service.RemoveByIdAsync(nonExistentPostId));
        Assert.Equal($"Post with ID 999 not found", exception.Message);
    }
    
    [Fact]
    public async Task GetAllAsync_ShouldReturnAllPosts()
    {
        // Arrange
        var expectedPostResponses = TestHelper.ExpectedPostResponses();
        
        // Act
        var postResponses = (await _service.GetAllAsync()).ToList();
        
        // Assert
        Assert.NotNull(postResponses);
        Assert.Equal(3, postResponses.Count);
        Assert.All(postResponses, postResponse =>
            Assert.True(new PostResponseComparer().Equals(expectedPostResponses.Single(p => p.Id == postResponse.Id), postResponse))
        );
    }
}