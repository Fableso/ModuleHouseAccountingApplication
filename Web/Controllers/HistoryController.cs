using Application.Abstractions;
using Domain.StronglyTypedIds;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

/// <summary>
/// Responsible for retrieving system history logs.
/// </summary>
[ApiController]
[Route("api/history")]
public class HistoryController : ControllerBase
{
    private readonly IHistoryService _historyService;

    /// <summary>
    /// Initializes a new instance of the <see cref="HistoryController"/> class.
    /// </summary>
    /// <param name="historyService">The history service dependency.</param>
    public HistoryController(IHistoryService historyService)
    {
        _historyService = historyService;
    }
    
    /// <summary>
    /// Retrieves the full history log for a specific house.
    /// </summary>
    /// <param name="houseId">The identifier of the house.</param>
    /// <param name="token">Cancellation token.</param>
    /// <returns>Returns the list of audit records for the house.</returns>
    /// <response code="200">Audit records retrieved successfully.</response>
    /// <response code="401">You're not authorized.</response>
    /// <response code="404">No records found for the provided house.</response>
    [Authorize(Policy = "SpectatorPolicy")]
    [HttpGet("house-full/{houseId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetFullHouseHistoryLogByIdAsync([FromRoute] HouseId houseId, CancellationToken token = default)
    {
        var audits = await _historyService.GetFullHouseHistoryLogByIdAsync(houseId, token);
        if (!audits.Any())
        {
            return NotFound("No records found for the provided house");
        }

        return Ok(audits);
    }
    
    /// <summary>
    /// Retrieves the main history log for a specific house.
    /// Main history log contains only the house entity changes and does not include related entities changes.
    /// </summary>
    /// <param name="houseId">The identifier of the house.</param>
    /// <param name="token">Cancellation token.</param>
    /// <returns>Returns the list of main audit records for the house.</returns>
    /// <response code="200">Audit records retrieved successfully.</response>
    /// <response code="401">You're not authorized.</response>
    /// <response code="404">No records found for the provided house.</response>
    [Authorize(Policy = "SpectatorPolicy")]
    [HttpGet("house-main/{houseId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetMainHouseHistoryLogByIdAsync([FromRoute] HouseId houseId, CancellationToken token = default)
    {
        var audits = await _historyService.GetMainHouseHistoryLogByIdAsync(houseId, token);
        if (!audits.Any())
        {
            return NotFound("No records found for the provided house");
        }

        return Ok(audits);
    }
    
    /// <summary>
    /// Retrieves the history log for a specific house week info.
    /// House week info history log contains only the events that happened to the specific house in a specific week.
    /// </summary>
    /// <param name="houseWeekInfoId">The identifier of the house week info.</param>
    /// <param name="token">Cancellation token.</param>
    /// <returns>Returns the list of audit records for the house week info.</returns>
    /// <response code="200">Audit records retrieved successfully.</response>
    /// <response code="401">You're not authorized.</response>
    /// <response code="404">No records found for the provided house week info.</response>
    [Authorize(Policy = "SpectatorPolicy")]
    [HttpGet("house-week/{houseWeekInfoId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetHouseWeekHistoryLogByIdAsync([FromRoute] HouseWeekInfoId houseWeekInfoId, CancellationToken token = default)
    {
        var audits = await _historyService.GetHouseWeekHistoryLogByIdAsync(houseWeekInfoId, token);
        if (!audits.Any())
        {
            return NotFound("No records found for the provided house week info");
        }

        return Ok(audits);
    }
}