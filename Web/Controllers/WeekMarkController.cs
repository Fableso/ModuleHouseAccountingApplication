using Application.Abstractions;
using Application.DTO.WeekMark.Request;
using Domain.StronglyTypedIds;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Validation.Extensions;

namespace Web.Controllers;

/// <summary>
/// Responsible for handling week mark related requests. Such as fetching, adding, updating, and deleting week marks.
/// </summary>
[ApiController]
[Route("api/week-mark")]
public class WeekMarkController : ControllerBase
{
    private readonly IWeekMarkService _weekMarkService;

    /// <summary>
    /// Initializes a new instance of the <see cref="WeekMarkController"/> class.
    /// </summary>
    /// <param name="weekMarkService">The week mark service dependency.</param>
    public WeekMarkController(IWeekMarkService weekMarkService)
    {
        _weekMarkService = weekMarkService;
    }
    
    /// <summary>
    /// Retrieves a week mark by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the week mark.</param>
    /// <returns>Returns the week mark details.</returns>
    /// <response code="200">Week mark retrieved successfully.</response>
    /// <response code="401">You're not authorized.</response>
    /// <response code="404">Week mark not found.</response>
    [Authorize(Policy = "SpectatorPolicy")]
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetWeekMarkById([FromRoute] WeekMarkId id)
    {
        var result = await _weekMarkService.GetByIdAsync(id);
        if(result is null)
        {
            return NotFound("Week mark not found");
        }
        return Ok(result);
    }
    
    /// <summary>
    /// Retrieves week marks by house week info identifier.
    /// </summary>
    /// <param name="id">The identifier of the house week info.</param>
    /// <returns>Returns a list of week marks.</returns>
    /// <response code="200">Week marks retrieved successfully.</response>
    /// <response code="401">You're not authorized.</response>
    /// <response code="404">No week marks found.</response>
    [Authorize(Policy = "SpectatorPolicy")]
    [HttpGet("week-info/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetWeekMarksByWeekInfoId([FromRoute] HouseWeekInfoId id)
    {
        var result = await _weekMarkService.GetWeekMarksByWeekInfoId(id);
        if(!result.Any())
        {
            return NotFound("No week marks found");
        }
        return Ok(result);
    }

    /// <summary>
    /// Creates a new week mark.
    /// </summary>
    /// <param name="request">The week mark creation details.</param>
    /// <returns>Returns the created week mark.</returns>
    /// <response code="201">Week mark created successfully.</response>
    /// <response code="400">Invalid request data.</response>
    /// <response code="401">You're not authorized.</response>
    /// <response code="403">You're not allowed to perform this action.</response>
    /// <response code="404">The house-week to which you're trying to add a week mark is not found.</response>
    [Authorize(Policy = "DefaultUserPolicy")]
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CreateWeekMark([FromBody] CreateWeekMarkRequest request)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.GetValidationErrors();
            return BadRequest(errors);
        }
        var result = await _weekMarkService.AddAsync(request);
        return CreatedAtAction("GetWeekMarkById", new { id = result.Id }, result);
    }

    /// <summary>
    /// Updates an existing week mark.
    /// </summary>
    /// <param name="request">The week mark update details.</param>
    /// <returns>Returns the updated week mark.</returns>
    /// <response code="200">Week mark updated successfully.</response>
    /// <response code="400">Invalid request data.</response>
    /// <response code="401">You're not authorized.</response>
    /// <response code="403">You're not allowed to perform this action.</response>
    /// <response code="404">Week mark not found.</response>
    [Authorize(Policy = "DefaultUserPolicy")]
    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateWeekMark([FromBody] UpdateWeekMarkRequest request)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.GetValidationErrors();
            return BadRequest(errors);
        }
        var result = await _weekMarkService.UpdateAsync(request);
        return Ok(result);
    }

    /// <summary>
    /// Deletes a week mark by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the week mark to delete.</param>
    /// <returns>Returns status code 204 No Content if successful.</returns>
    /// <response code="204">Week mark deleted successfully.</response>
    /// <response code="401">You're not authorized.</response>
    /// <response code="403">You're not allowed to perform this action.</response>
    /// <response code="404">Week mark you're trying to delete is not found.</response>
    [Authorize(Policy = "DefaultUserPolicy")]
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteWeekMark([FromRoute] WeekMarkId id)
    {
        await _weekMarkService.RemoveByIdAsync(id);
        return Ok();
    }
}