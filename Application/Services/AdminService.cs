using System.Text;
using Application.Abstractions;
using Application.DTO.Identity;
using Application.Exceptions;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;

namespace Application.Services;

public class AdminService : IAdminService
{
    private const string ConfirmationEmailMessage = "Please confirm your account by clicking this link: <a href=\"{0}\">Confirm Email</a>";
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly ILogger<AdminService> _logger;
    private readonly IEmailSender _emailSender;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AdminService(
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager,
        ILogger<AdminService> logger,
        IEmailSender emailSender,
        IHttpContextAccessor httpContextAccessor)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _logger = logger;
        _emailSender = emailSender;
        _httpContextAccessor = httpContextAccessor;
    }
    
    public async Task CreateUserAsync(CreateUserRequest model)
    {
        if (!await _roleManager.RoleExistsAsync(model.Role))
        {
            _logger.LogWarning("{Action}: Attempt to create user with non-existing role {Role}",
                nameof(CreateUserAsync), model.Role);
            throw new ArgumentException($"Role {model.Role} does not exist");
        }

        var user = new ApplicationUser
        {
            UserName = model.Email,
            Email = model.Email,
        };

        var result = await _userManager.CreateAsync(user, model.Password);
        if (result.Succeeded)
        {
            await _userManager.AddToRoleAsync(user, model.Role);
            await SendConfirmationEmailAsync(user);
        }
        else
        {
            var errorMessage = string.Join(", ", result.Errors);
            _logger.LogError("{Action}: Failed to create user: {ErrorMessage}",
                nameof(CreateUserAsync), errorMessage);
            throw new IdentityException($"Failed to create user: {errorMessage}");
        }
    }
    
    public async Task PromoteUserAsync(string userEmail)
    {
        var user = await _userManager.FindByEmailAsync(userEmail);
        if (user != null)
        {
            if (await _userManager.IsInRoleAsync(user, "Spectator"))
            {
                await _userManager.RemoveFromRoleAsync(user, "Spectator");
                await _userManager.AddToRoleAsync(user, "DefaultUser");
            }
            else
            {
                _logger.LogWarning("{Action}: Attempt to promote user {UserEmail} failed: user is not in the Spectator role",
                    nameof(PromoteUserAsync), userEmail);
                throw new InvalidOperationException("User is not in the Spectator role and cannot be promoted.");
            }
        }
        else
        {
            _logger.LogWarning("{Action}: Attempt to promote user {UserEmail} failed: user not found",
                nameof(PromoteUserAsync), userEmail);
            throw new EntityNotFoundException($"User with email {userEmail} not found.");
        }
    }
    
    public async Task DemoteUserAsync(string userEmail)
    {
        var user = await _userManager.FindByEmailAsync(userEmail);
        if (user is null)
        {
            _logger.LogWarning("{Action}: Attempt to demote user {UserEmail} failed: user not found",
                nameof(DemoteUserAsync), userEmail);
            throw new EntityNotFoundException($"User with email {userEmail} not found.");
        }
        
        if (await _userManager.IsInRoleAsync(user, "DefaultUser"))
        {
            await _userManager.RemoveFromRoleAsync(user, "DefaultUser");
            await _userManager.AddToRoleAsync(user, "Spectator");
        }
        else
        {
            _logger.LogWarning("{Action}:Attempt to demote user {UserEmail} failed: user is not in the DefaultUser role",
                nameof(DemoteUserAsync), userEmail);
            throw new InvalidOperationException("User is not in the DefaultUser role and cannot be demoted.");
        }
        
    }
    
    public async Task DeleteUserAsync(string userEmail)
    {
        var user = await _userManager.FindByEmailAsync(userEmail);
        if (user is null)
        {
            _logger.LogWarning("{Action}: Attempt to delete user {UserEmail} failed: user not found",
                nameof(DeleteUserAsync), userEmail);
            throw new EntityNotFoundException($"User with email {userEmail} not found.");
        }
        
        await _userManager.DeleteAsync(user);
    }
    
    private async Task SendConfirmationEmailAsync(
        ApplicationUser user)
    {
        var request = _httpContextAccessor.HttpContext?.Request;
        if (request is null)
        {
            throw new InvalidOperationException("Unable to generate confirmation email link without HTTP context");
        }
        var scheme = request.Scheme;
        var host = request.Host;
        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
        var confirmationLink = $"{scheme}://{host}/api/auth/ConfirmEmail?userId={user.Id}&token={encodedToken}";
        var message = string.Format(ConfirmationEmailMessage, confirmationLink);
        if (user.Email is null)
        {
            throw new InvalidOperationException("Unable to send confirmation email to user with no email address");
        }
        await _emailSender.SendEmailAsync(user.Email, "Confirm your email", message);
    }
}