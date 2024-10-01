using Application.Abstractions;
using Application.DTO.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Validation.Extensions;

namespace Web.Controllers;

[ApiController]
[Route("api/admin")]
[Authorize(Policy = "AdminPolicy")]
public class AdminController : ControllerBase
{
    private readonly IAdminService _adminService;

    public AdminController(IAdminService adminService)
    {
        _adminService = adminService;
    }
    
    [HttpPost("create-user")]
    public async Task<IActionResult> CreateUser(CreateUserRequest request)
    {
        if (ModelState.IsValid)
        {
            await _adminService.CreateUserAsync(request);
            return Ok();
        }
        
        var errors = ModelState.GetValidationErrors();
        return BadRequest(errors);
    }
    
    [HttpPost("delete-user")]
    public async Task<IActionResult> DeleteUser(string userEmail)
    {
        await _adminService.DeleteUserAsync(userEmail);
        return Ok();
    }
    
    [HttpPost("promote-user")]
    public async Task<IActionResult> PromoteUser(string userEmail)
    {
        try
        {
            await _adminService.PromoteUserAsync(userEmail);
            return Ok();
        }
        catch (InvalidOperationException e)
        {
            return BadRequest(e.Message);
        }
    }
    
    [HttpPost("demote-user")]
    public async Task<IActionResult> DemoteUser(string userEmail)
    {
        try
        {
            await _adminService.DemoteUserAsync(userEmail);
            return Ok();
        }
        catch (InvalidOperationException e)
        {
            return BadRequest(e.Message);
        }
    }
}