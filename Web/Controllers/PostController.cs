using Application.Abstractions;
using Application.DTO.Post.Request;
using Domain.StronglyTypedIds;
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
}