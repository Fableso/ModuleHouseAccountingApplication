using Application.Abstractions;
using Application.DTO.Post.Request;
using Domain.StronglyTypedIds;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Validation.Extensions;

namespace Web.Controllers;

/// <summary>
/// Responsible for post-related operations such as fetching, adding, updating, and deleting posts.
/// </summary>
[ApiController]
[Route("api/post")]
public class PostController : ControllerBase
{
    private readonly IPostService _postService;

    /// <summary>
    /// Initializes a new instance of the <see cref="PostController"/> class.
    /// </summary>
    /// <param name="postService">The post service dependency.</param>
    public PostController(IPostService postService)
    {
        _postService = postService;
    }
    
    /// <summary>
    /// Retrieves all posts.
    /// </summary>
    /// <param name="token">Cancellation token.</param>
    /// <returns>Returns a list of posts.</returns>
    /// <response code="200">Posts retrieved successfully.</response>
    /// <response code="401">You're not authorized.</response>
    /// <response code="404">No posts found.</response>
    [Authorize(Policy = "SpectatorPolicy")]
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAllPostsAsync(CancellationToken token = default)
    {
        var posts = await _postService.GetAllAsync(token);
        if (!posts.Any())
        {
            return NotFound("No posts found");
        }

        return Ok(posts);
    }
    
    /// <summary>
    /// Retrieves a post by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the post.</param>
    /// <param name="token">Cancellation token.</param>
    /// <returns>Returns the post details.</returns>
    /// <response code="200">Post retrieved successfully.</response>
    /// <response code="401">You're not authorized.</response>
    /// <response code="404">Post not found.</response>
    [Authorize(Policy = "SpectatorPolicy")]
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetPostByIdAsync([FromRoute] PostId id, CancellationToken token = default)
    {
        var post = await _postService.GetByIdAsync(id, token);
        if (post == null)
        {
            return NotFound("Post not found");
        }

        return Ok(post);
    }
    
    /// <summary>
    /// Adds a new post.
    /// </summary>
    /// <param name="postRequest">The post creation details.</param>
    /// <param name="token">Cancellation token.</param>
    /// <returns>Returns the created post.</returns>
    /// <response code="201">Post created successfully.</response>
    /// <response code="400">Invalid request data.</response>
    /// <response code="401">You're not authorized.</response>
    /// <response code="403">You're not allowed to perform this action.</response>
    /// <response code="409">Post with the same name already exists.</response>
    [Authorize(Policy = "DefaultUserPolicy")]
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
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
    
    /// <summary>
    /// Updates an existing post.
    /// </summary>
    /// <param name="postRequest">The post update details.</param>
    /// <param name="token">Cancellation token.</param>
    /// <returns>Returns the updated post.</returns>
    /// <response code="200">Post updated successfully.</response>
    /// <response code="400">Invalid request data.</response>
    /// <response code="401">You're not authorized.</response>
    /// <response code="403">You're not allowed to perform this action.</response>
    /// <response code="409">Post with the same name already exists.</response>
    [Authorize(Policy = "DefaultUserPolicy")]
    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
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
    
    /// <summary>
    /// Deletes a post by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the post to delete.</param>
    /// <param name="token">Cancellation token.</param>
    /// <returns>Returns status code 204 No Content if successful.</returns>
    /// <response code="204">Post deleted successfully.</response>
    /// <response code="401">You're not authorized.</response>
    /// <response code="403">You're not allowed to perform this action.</response>
    [Authorize(Policy = "DefaultUserPolicy")]
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> RemovePostByIdAsync([FromRoute] PostId id, CancellationToken token = default)
    {
        await _postService.RemoveByIdAsync(id, token);
        return NoContent();
    }
}