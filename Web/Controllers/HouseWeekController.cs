using Application.Abstractions;
using Application.DTO.HouseWeekInfo.Request;
using Domain.StronglyTypedIds;
using Domain.ValueObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Validation.Extensions;

namespace Web.Controllers;

/// <summary>
/// Responsible for handling house week info related requests.
/// </summary>
[ApiController]
[Route("api/house-week")]
public class HouseWeekController : ControllerBase
{
    private readonly IHouseWeekInfoService _houseWeekService;

    /// <summary>
    /// Initializes a new instance of the <see cref="HouseWeekController"/> class.
    /// </summary>
    /// <param name="houseWeekService">The house week info service dependency.</param>
    public HouseWeekController(IHouseWeekInfoService houseWeekService)
    {
        _houseWeekService = houseWeekService;
    }
    
    /// <summary>
    /// Retrieves all house weeks for a specific house.
    /// </summary>
    /// <param name="houseId">The identifier of the house.</param>
    /// <param name="token">Cancellation token.</param>
    /// <returns>Returns a list of house weeks.</returns>
    /// <response code="200">House weeks retrieved successfully.</response>
    /// <response code="401">You're not authorized.</response>
    [Authorize(Policy = "SpectatorPolicy")]
    [HttpGet("house/{houseId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetHouseWeeksForHouseAsync([FromRoute] HouseId houseId, CancellationToken token = default)
    {
        var houseWeeks = await _houseWeekService.GetHouseInfosForHouseAsync(houseId, token);
        return Ok(houseWeeks);
    }
    
    /// <summary>
    /// Retrieves house weeks for a specific house within a date range.
    /// </summary>
    /// <param name="houseId">The identifier of the house.</param>
    /// <param name="startDate">The start date.</param>
    /// <param name="endDate">The end date.</param>
    /// <param name="token">Cancellation token.</param>
    /// <returns>Returns a list of house weeks.</returns>
    /// <response code="200">House weeks retrieved successfully.</response>
    /// <response code="400">Invalid date range.</response>
    /// <response code="401">You're not authorized.</response>
    /// <response code="404">House with the specified id is not found</response>
    [Authorize(Policy = "SpectatorPolicy")]
    [HttpGet("house/{houseId}/range")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetHouseWeeksForHouseInTimeSpanAsync(
        [FromRoute] HouseId houseId,
        [FromQuery] DateOnly startDate,
        [FromQuery] DateOnly endDate,
        CancellationToken token = default)
    {
        var dateSpan = DateSpan.Create(startDate, endDate);
        if (dateSpan.IsFailed)
        {
            return BadRequest(dateSpan.Errors);
        }
        var houseWeeks = await _houseWeekService.GetHouseInfosForHouseInTimeSpanAsync(houseId, dateSpan.Value, token);
        return Ok(houseWeeks);
    }
    
    /// <summary>
    /// Retrieves house weeks within a date range.
    /// </summary>
    /// <param name="startDate">The start date.</param>
    /// <param name="endDate">The end date.</param>
    /// <param name="token">Cancellation token.</param>
    /// <returns>Returns a list of house weeks.</returns>
    /// <response code="200">House weeks retrieved successfully.</response>
    /// <response code="400">Invalid date range.</response>
    /// <response code="401">You're not authorized.</response>
    [Authorize(Policy = "SpectatorPolicy")]
    [HttpGet("range")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetHouseWeeksInTimeSpanAsync([FromQuery] DateOnly startDate, [FromQuery] DateOnly endDate, CancellationToken token = default)
    {
        var dateSpan = DateSpan.Create(startDate, endDate);
        if (dateSpan.IsFailed)
        {
            return BadRequest(dateSpan.Errors);
        }
        var houseWeeks = await _houseWeekService.GetHouseInfosInTimeSpanAsync(dateSpan.Value, token);
        return Ok(houseWeeks);
    }

    /// <summary>
    /// Retrieves a house week by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the house week.</param>
    /// <param name="token">Cancellation token.</param>
    /// <returns>Returns the house week details.</returns>
    /// <response code="200">House week retrieved successfully.</response>
    /// <response code="404">House week not found.</response>
    /// <response code="401">You're not authorized.</response>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetHouseWeekByIdAsync([FromRoute] HouseWeekInfoId id, CancellationToken token = default)
    {
        var houseWeek = await _houseWeekService.GetByIdAsync(id, token);
        if (houseWeek == null)
        {
            return NotFound("House week not found");
        }

        return Ok(houseWeek);
    }

    /// <summary>
    /// Adds a new house week.
    /// </summary>
    /// <param name="houseWeekRequest">The house week creation details.</param>
    /// <param name="token">Cancellation token.</param>
    /// <returns>Returns the created house week.</returns>
    /// <response code="201">House week created successfully.</response>
    /// <response code="400">Invalid request data.</response>
    /// <response code="401">You're not authorized.</response>
    /// <response code="403">You're not allowed to perform this action.</response>
    /// <response code="404">The house to which you're trying to add a week-info is not found.</response>
    [Authorize(Policy = "DefaultUserPolicy")]
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AddHouseWeekAsync([FromBody] CreateHouseWeekInfoRequest houseWeekRequest, CancellationToken token = default)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.GetValidationErrors();
            return BadRequest(errors);
        }
        var houseWeek = await _houseWeekService.AddAsync(houseWeekRequest, token);
        return CreatedAtAction("GetHouseWeekById", new { id = houseWeek.Id }, houseWeek);
    }

    /// <summary>
    /// Updates an existing house week.
    /// </summary>
    /// <param name="houseWeekRequest">The house week update details.</param>
    /// <param name="token">Cancellation token.</param>
    /// <returns>Returns status code 204 No Content if successful.</returns>
    /// <response code="204">House week updated successfully.</response>
    /// <response code="400">Invalid request data.</response>
    /// <response code="401">You're not authorized.</response>
    /// <response code="403">You're not allowed to perform this action.</response>
    /// <response code="404">House week not found.</response>
    [Authorize(Policy = "DefaultUserPolicy")]
    [HttpPut]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateHouseWeekAsync([FromBody] UpdateHouseWeekInfoRequest houseWeekRequest, CancellationToken token = default)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.GetValidationErrors();
            return BadRequest(errors);
        }
        await _houseWeekService.UpdateStatusAsync(houseWeekRequest, token);
        return NoContent();
    }

    /// <summary>
    /// Deletes a house week by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the house week to delete.</param>
    /// <param name="token">Cancellation token.</param>
    /// <returns>Returns status code 204 No Content if successful.</returns>
    /// <response code="204">House week deleted successfully.</response>
    /// <response code="401">You're not authorized.</response>
    /// <response code="403">You're not allowed to perform this action.</response>
    /// <response code="404">House week not found.</response>
    [Authorize(Policy = "DefaultUserPolicy")]
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteHouseWeekAsync([FromRoute] HouseWeekInfoId id, CancellationToken token = default)
    {
        await _houseWeekService.RemoveByIdAsync(id, token);
        return NoContent();
    }
}