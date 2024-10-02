using Application.Abstractions;
using Application.DTO.House.Request;
using Domain.StronglyTypedIds;
using Domain.ValueObjects;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Validation.Extensions;

namespace Web.Controllers;

/// <summary>
/// Responsible for house-related operations such as fetching, adding, updating, and deleting houses.
/// </summary>
[ApiController]
[Route("api/house")]
public class HouseController : ControllerBase
{
    private readonly IHouseService _houseService;

    /// <summary>
    /// Initializes a new instance of the <see cref="HouseController"/> class.
    /// </summary>
    /// <param name="houseService">The house service dependency.</param>
    public HouseController(IHouseService houseService)
    {
        _houseService = houseService;
    }

    /// <summary>
    /// Retrieves houses within a specified date range.
    /// </summary>
    /// <param name="startDate">The start date of the range.</param>
    /// <param name="endDate">The end date of the range.</param>
    /// <param name="token">Cancellation token.</param>
    /// <returns>Returns a list of houses within the date range.</returns>
    /// <response code="200">Houses retrieved successfully.</response>
    /// <response code="400">Invalid date range.</response>
    /// <response code="401">You're not authorized.</response>
    /// <response code="404">No houses found in the given date range.</response>
    [Authorize(Policy = "SpectatorPolicy")]
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetHousesInDateRange([FromQuery] DateOnly startDate, [FromQuery] DateOnly endDate, CancellationToken token = default)
    {
        var dateSpanResult = DateSpan.Create(startDate, endDate);
        if (dateSpanResult.IsFailed)
        {
            return BadRequest(dateSpanResult.Errors);
        }

        var houses = await _houseService.GetHousesInDateRangeAsync(DateSpan.Create(startDate, endDate).Value, token);
        if (!houses.Any())
        {
            return NotFound("No houses found in the given date range");
        }

        return Ok(houses);
    }

    /// <summary>
    /// Retrieves a house by its model identifier.
    /// </summary>
    /// <param name="model">The model identifier of the house.</param>
    /// <param name="token">Cancellation token.</param>
    /// <returns>Returns the house details.</returns>
    /// <response code="200">House retrieved successfully.</response>
    /// <response code="404">House not found.</response>
    /// <response code="401">You're not authorized.</response>
    [Authorize(Policy = "SpectatorPolicy")]
    [HttpGet("{model}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetHouseByModelAsync([FromRoute] HouseId model, CancellationToken token = default)
    {
        var house =  await _houseService.GetByIdAsync(model, token);
        if (house is null)
        {
            return NotFound($"House with model {model.Value} not found");
        }

        return Ok(house);
    }
    
    /// <summary>
    /// Adds a new house.
    /// </summary>
    /// <param name="houseRequest">The house creation details.</param>
    /// <param name="token">Cancellation token.</param>
    /// <returns>Returns the created house.</returns>
    /// <response code="201">House created successfully.</response>
    /// <response code="400">Invalid request data.</response>
    /// <response code="401">You're not authorized.</response>
    /// <response code="403">You're not allowed to perform this action.</response>
    /// <response code="404">One or more post that were assigned to the house do not exist.</response>
    /// <response code="409">House with the same model already exists.</response>
    [Authorize(Policy = "DefaultUserPolicy")]
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> AddHouseAsync([FromBody] CreateHouseRequest houseRequest, CancellationToken token = default)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.GetValidationErrors();
            return BadRequest(errors);
        }
        var house = await _houseService.AddAsync(houseRequest, token);
        return CreatedAtAction("GetHouseByModel", new { model = house.Model.Value }, house);
    }
    
    /// <summary>
    /// Updates an existing house.
    /// </summary>
    /// <param name="houseRequest">The house update details.</param>
    /// <param name="token">Cancellation token.</param>
    /// <returns>Returns status code 204 No Content if successful.</returns>
    /// <response code="204">House updated successfully.</response>
    /// <response code="400">Invalid request data.</response>
    /// <response code="401">You're not authorized.</response>
    /// <response code="403">You're not allowed to perform this action.</response>
    /// <response code="404">Whether the house or posts assigned to it not found.</response>
    [Authorize(Policy = "DefaultUserPolicy")]
    [HttpPut]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateHouseAsync([FromBody] UpdateHouseRequest houseRequest, CancellationToken token = default)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.GetValidationErrors();

            return BadRequest(errors);
        }
        await _houseService.UpdateAsync(houseRequest, token);
        return NoContent();
    }
    
    /// <summary>
    /// Deletes a house by its model identifier.
    /// </summary>
    /// <param name="model">The model identifier of the house to delete.</param>
    /// <param name="token">Cancellation token.</param>
    /// <returns>Returns status code 204 No Content if successful.</returns>
    /// <response code="204">House deleted successfully.</response>
    /// <response code="401">You're not authorized.</response>
    /// <response code="403">You're not allowed to perform this action.</response>
    /// <response code="404">The house you're trying to delete is not found.</response>
    [Authorize(Policy = "DefaultUserPolicy")]
    [HttpDelete("{model}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteHouseAsync([FromRoute] HouseId model, CancellationToken token = default)
    {
        await _houseService.RemoveByIdAsync(model, token);
        return NoContent();
    }
}