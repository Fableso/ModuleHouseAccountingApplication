using Application.Abstractions;
using Domain.StronglyTypedIds;
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
}