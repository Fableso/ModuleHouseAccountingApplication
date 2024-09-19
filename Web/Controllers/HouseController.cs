using Application.Abstractions;
using Application.DTO.House.Request;
using Domain.StronglyTypedIds;
using Domain.ValueObjects;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Web.Validation.Extensions;

namespace Web.Controllers;

[ApiController]
[Route("api/house")]
public class HouseController : ControllerBase
{
    private readonly IHouseService _houseService;

    public HouseController(IHouseService houseService)
    {
        _houseService = houseService;
    }

    [HttpGet]
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

    [HttpGet("{model}")]
    public async Task<IActionResult> GetHouseByModelAsync([FromRoute] HouseId model, CancellationToken token = default)
    {
        var house =  await _houseService.GetByIdAsync(model, token);
        if (house is null)
        {
            return NotFound("No houses found in the given date range");
        }

        return Ok(house);
    }
    
    [HttpPost]
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
    
    [HttpPut]
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


    [HttpDelete("{model}")]
    public async Task<IActionResult> DeleteHouseAsync([FromRoute] HouseId model, CancellationToken token = default)
    {
        await _houseService.RemoveByIdAsync(model, token);
        return NoContent();
    }
}