using Application.Abstractions;
using Application.DTO.WeekMark.Request;
using Domain.StronglyTypedIds;
using Microsoft.AspNetCore.Mvc;
using Web.Validation.Extensions;

namespace Web.Controllers;

[ApiController]
[Route("api/week-mark")]
public class WeekMarkController : ControllerBase
{
    private readonly IWeekMarkService _weekMarkService;

    public WeekMarkController(IWeekMarkService weekMarkService)
    {
        _weekMarkService = weekMarkService;
    }
    
    [HttpGet("{id}")] 
    public async Task<IActionResult> GetWeekMarkById([FromRoute] WeekMarkId id)
    {
        var result = await _weekMarkService.GetByIdAsync(id);
        if(result is null)
        {
            return NotFound("Week mark not found");
        }
        return Ok(result);
    }
    
    [HttpGet("week-info/{id}")]
    public async Task<IActionResult> GetWeekMarksByWeekInfoId([FromRoute] HouseWeekInfoId id)
    {
        var result = await _weekMarkService.GetWeekMarksByWeekInfoId(id);
        if(!result.Any())
        {
            return NotFound("No week marks found");
        }
        return Ok(result);
    }

    [HttpPost]
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

    [HttpPut]
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

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteWeekMark([FromRoute] WeekMarkId id)
    {
        await _weekMarkService.RemoveByIdAsync(id);
        return Ok();
    }
}