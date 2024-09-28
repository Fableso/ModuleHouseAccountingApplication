using Application.Abstractions;
using Domain.StronglyTypedIds;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

[ApiController]
[Route("api/history")]
public class HistoryController : ControllerBase
{
    private readonly IHistoryService _historyService;

    public HistoryController(IHistoryService historyService)
    {
        _historyService = historyService;
    }
    
    [Authorize(Policy = "SpectatorPolicy")]
    [HttpGet("house-full/{houseId}")]
    public async Task<IActionResult> GetFullHouseHistoryLogByIdAsync([FromRoute] HouseId houseId, CancellationToken token = default)
    {
        var audits = await _historyService.GetFullHouseHistoryLogByIdAsync(houseId, token);
        if (!audits.Any())
        {
            return NotFound("No records found for the provided house");
        }

        return Ok(audits);
    }
    
    [Authorize(Policy = "SpectatorPolicy")]
    [HttpGet("house-main/{houseId}")]
    public async Task<IActionResult> GetMainHouseHistoryLogByIdAsync([FromRoute] HouseId houseId, CancellationToken token = default)
    {
        var audits = await _historyService.GetMainHouseHistoryLogByIdAsync(houseId, token);
        if (!audits.Any())
        {
            return NotFound("No records found for the provided house");
        }

        return Ok(audits);
    }
    
    [Authorize(Policy = "SpectatorPolicy")]
    [HttpGet("house-week/{houseWeekInfoId}")]
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