using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Domain.Entities;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Web.Exceptions;

namespace Web.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    public AuthController(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user is not null && await _userManager.CheckPasswordAsync(user, request.Password))
        {
            var userRoles = await _userManager.GetRolesAsync(user);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName ?? throw new InvalidOperationException("User name is null")),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            
            claims.AddRange(userRoles.Select(role => new Claim(ClaimTypes.Role, role)));
            
            var token = GetToken(claims);
            
            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiration = token.ValidTo
            });
        }

        return Unauthorized();
    }
    
    private static JwtSecurityToken GetToken(IEnumerable<Claim> claims)
    {
        var secretKey = Environment.GetEnvironmentVariable("JWT_SECRET_KEY");
        var authSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(secretKey ?? throw new ConfigurationException("JWT_SECRET_KEY is not set")));

        var token = new JwtSecurityToken(
            expires: DateTime.UtcNow.AddHours(3),
            issuer: Environment.GetEnvironmentVariable("JWT_ISSUER") ?? throw new ConfigurationException("JWT_ISSUER is not set"),
            audience:Environment.GetEnvironmentVariable("JWT_AUDIENCE") ?? throw new ConfigurationException("JWT_AUDIENCE is not set"),
            claims: claims,
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
        );

        return token;
    }
}
