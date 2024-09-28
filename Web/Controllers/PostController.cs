using Application.Abstractions;
using Application.DTO.Post.Request;
using Domain.StronglyTypedIds;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Validation.Extensions;

namespace Web.Controllers;

[ApiController]
[Route("api/post")]
public class PostController : ControllerBase
{
    private readonly IPostService _postService;

    public PostController(IPostService postService)
    {
        _postService = postService;
    }
    
    [Authorize(Policy = "SpectatorPolicy")]
    [HttpGet]
    public async Task<IActionResult> GetAllPostsAsync(CancellationToken token = default)
    {
        var posts = await _postService.GetAllAsync(token);
        if (!posts.Any())
        {
            return NotFound("No posts found");
        }

        return Ok(posts);
    }
    
    [Authorize(Policy = "SpectatorPolicy")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetPostByIdAsync([FromRoute] PostId id, CancellationToken token = default)
    {
        var post = await _postService.GetByIdAsync(id, token);
        if (post == null)
        {
            return NotFound("Post not found");
        }

        return Ok(post);
    }
    
    [Authorize(Policy = "DefaultUserPolicy")]
    [HttpPost]
    public async Task<IActionResult> AddPostAsync([FromBody] CreatePostRequest postRequest, CancellationToken token = default)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.GetValidationErrors();
            return BadRequest(errors);
        }
        var post = await _postService.AddAsync(postRequest, token);
        return CreatedAtAction("GetPostById", new { id = post.Id }, post);
    }
    
    [Authorize(Policy = "DefaultUserPolicy")]
    [HttpPut]
    public async Task<IActionResult> UpdatePostAsync([FromBody] UpdatePostRequest postRequest, CancellationToken token = default)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.GetValidationErrors();
            return BadRequest(errors);
        }
        var post = await _postService.UpdateAsync(postRequest, token);
        return Ok(post);
    }
    
    [Authorize(Policy = "DefaultUserPolicy")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> RemovePostByIdAsync([FromRoute] PostId id, CancellationToken token = default)
    {
        await _postService.RemoveByIdAsync(id, token);
        return NoContent();
    }
}