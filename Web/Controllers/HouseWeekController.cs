using Application.Abstractions;
using Application.DTO.HouseWeekInfo.Request;
using Domain.StronglyTypedIds;
using Domain.ValueObjects;
using Microsoft.AspNetCore.Mvc;
using Web.Validation.Extensions;

namespace Web.Controllers;

[ApiController]
[Route("api/house-week")]
public class HouseWeekController : ControllerBase
{
    private readonly IHouseWeekInfoService _houseWeekService;

    public HouseWeekController(IHouseWeekInfoService houseWeekService)
    {
        _houseWeekService = houseWeekService;
    }
    
    [HttpGet("house/{houseId}")]
    public async Task<IActionResult> GetHouseWeeksForHouseAsync([FromRoute] HouseId houseId, CancellationToken token = default)
    {
        var houseWeeks = await _houseWeekService.GetHouseInfosForHouseAsync(houseId, token);
        return Ok(houseWeeks);
    }
    
    [HttpGet("house/{houseId}/range")]
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
    
    [HttpGet("range")]
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

    [HttpGet("{id}")]
    public async Task<IActionResult> GetHouseWeekByIdAsync([FromRoute] HouseWeekInfoId id, CancellationToken token = default)
    {
        var houseWeek = await _houseWeekService.GetByIdAsync(id, token);
        if (houseWeek == null)
        {
            return NotFound("House week not found");
        }

        return Ok(houseWeek);
    }

    [HttpPost]
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

    [HttpPut]
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

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteHouseWeekAsync([FromRoute] HouseWeekInfoId id, CancellationToken token = default)
    {
        await _houseWeekService.RemoveByIdAsync(id, token);
        return NoContent();
    }
}