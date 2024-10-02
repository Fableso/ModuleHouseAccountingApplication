using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.DTO.Auth;
using Application.Exceptions;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.IdentityModel.Tokens;

namespace Web.Controllers;

/// <summary>
/// Controller responsible for authentication operations such as login and email confirmation.
/// </summary>
[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;

    /// <summary>
    /// Initializes a new instance of the <see cref="AuthController"/> class.
    /// </summary>
    /// <param name="userManager">User manager for handling user-related operations.</param>
    public AuthController(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }
    
    /// <summary>
    /// Authenticates a user and returns a JWT token if successful.
    /// </summary>
    /// <param name="request">An object containing the user's email and password.</param>
    /// <returns>JWT token and expiration date if authentication is successful.</returns>
    /// <response code="200">Returns the JWT token and expiration date.</response>
    /// <response code="401">Invalid email or password, or email not confirmed.</response>
    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LoginResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null || !await _userManager.CheckPasswordAsync(user, request.Password))
        {
            return Unauthorized("Invalid email or password.");
        }

        if (!await _userManager.IsEmailConfirmedAsync(user))
        {
            return Unauthorized("You need to confirm your email before logging in.");
        }

        var userRoles = await _userManager.GetRolesAsync(user);
        var claims = new List<Claim>
        {
            new Claim("UserId", user.Id),
            new Claim(ClaimTypes.Name, user.UserName ?? throw new InvalidOperationException("User name is null")),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };
        claims.AddRange(userRoles.Select(role => new Claim(ClaimTypes.Role, role)));

        var token = GetToken(claims);

        return Ok(
            new LoginResponse(
                new JwtSecurityTokenHandler().WriteToken(token),
                token.ValidTo)
            );
    }

    /// <summary>
    /// Confirms a user's email address using a confirmation token.
    /// </summary>
    /// <param name="userId">The user's unique identifier.</param>
    /// <param name="token">The email confirmation token.</param>
    /// <returns>A message indicating whether the email was confirmed successfully.</returns>
    /// <response code="200">Email confirmed successfully.</response>
    /// <response code="400">User ID or token is missing, or email confirmation failed.</response>
    /// <response code="404">User not found.</response>
    [HttpGet("ConfirmEmail")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ConfirmEmail(string userId, string token)
    {
        if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
        {
            return BadRequest("User ID and Token are required");
        }

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return NotFound("User not found");
        }

        var decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));
        var result = await _userManager.ConfirmEmailAsync(user, decodedToken);

        if (result.Succeeded)
        {
            return Ok("Email confirmed successfully!");
        }

        return BadRequest("Email confirmation failed");
    }

    /// <summary>
    /// Generates a JWT token based on the provided claims.
    /// </summary>
    /// <param name="claims">A collection of claims to include in the token.</param>
    /// <returns>A JWT security token.</returns>
    private static JwtSecurityToken GetToken(IEnumerable<Claim> claims)
    {
        var secretKey = Environment.GetEnvironmentVariable("JWT_SECRET_KEY");
        var authSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(secretKey ?? throw new ConfigurationException("JWT_SECRET_KEY is not set")));

        var token = new JwtSecurityToken(
            expires: DateTime.UtcNow.AddHours(3),
            issuer: Environment.GetEnvironmentVariable("JWT_ISSUER") ?? throw new ConfigurationException("JWT_ISSUER is not set"),
            audience: Environment.GetEnvironmentVariable("JWT_AUDIENCE") ?? throw new ConfigurationException("JWT_AUDIENCE is not set"),
            claims: claims,
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
        );

        return token;
    }
}
