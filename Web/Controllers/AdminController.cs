using Application.Abstractions;
using Application.DTO.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Validation.Extensions;

namespace Web.Controllers;

/// <summary>
/// Controller responsible for handling admin actions such as creating/deleting system users and also promoting/demoting them.
/// </summary>
[ApiController]
[Route("api/admin")]
[Authorize(Policy = "AdminPolicy")]
public class AdminController : ControllerBase
{
    private readonly IAdminService _adminService;

    /// <summary>
    /// Initializes a new instance of the <see cref="AdminController"/> class.
    /// </summary>
    /// <param name="adminService">The admin service dependency.</param>
    public AdminController(IAdminService adminService)
    {
        _adminService = adminService;
    }
    
    /// <summary>
    /// Creates a new user.
    /// </summary>
    /// <param name="request">The user creation details.</param>
    /// <returns>Returns status code 200 OK if successful; otherwise, 400 Bad Request.</returns>
    /// <response code="200">User created successfully.</response>
    /// <response code="400">Invalid request data.</response>
    /// <response code="401">You're not authorized.</response>
    /// <response code="403">You're not allowed to perform this action.</response>
    [HttpPost("create-user")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
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
    
    /// <summary>
    /// Deletes an existing user.
    /// </summary>
    /// <param name="userEmail">The email of the user to delete.</param>
    /// <returns>Returns status code 200 OK if successful.</returns>
    /// <response code="200">User deleted successfully.</response>
    /// <response code="401">You're not authorized.</response>
    /// <response code="403">You're not allowed to perform this action.</response>
    /// <response code="404">User you're trying to delete is not found</response>
    [HttpPost("delete-user")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteUser(string userEmail)
    {
        await _adminService.DeleteUserAsync(userEmail);
        return Ok();
    }
    
    /// <summary>
    /// Promotes a user to a higher role.
    /// </summary>
    /// <param name="userEmail">The email of the user to promote.</param>
    /// <returns>Returns status code 200 OK if successful; otherwise, 400 Bad Request.</returns>
    /// <response code="200">User promoted successfully.</response>
    /// <response code="400">It's impossible to promote the user you've specified</response>
    /// <response code="401">You're not authorized.</response>
    /// <response code="403">You're not allowed to perform this action.</response>
    /// <response code="404">User you're trying to promote is not found</response>
    [HttpPost("promote-user")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
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
    
    /// <summary>
    /// Demotes a user to a lower role.
    /// </summary>
    /// <param name="userEmail">The email of the user to demote.</param>
    /// <returns>Returns status code 200 OK if successful; otherwise, 400 Bad Request.</returns>
    /// <response code="200">User demoted successfully.</response>
    /// <response code="400">It's impossible to demote the user you've specified</response>
    /// <response code="401">You're not authorized.</response>
    /// <response code="403">You're not allowed to perform this action.</response>
    /// <response code="404">User you're trying to demote is not found</response>
    [HttpPost("demote-user")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
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