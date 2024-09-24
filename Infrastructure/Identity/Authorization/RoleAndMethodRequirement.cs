using Microsoft.AspNetCore.Authorization;

namespace Infrastructure.Identity.Authorization;

public class RoleAndMethodRequirement : IAuthorizationRequirement
{
    public string[] AllowedRoles { get; }
    public string[] AllowedMethods { get; }

    public RoleAndMethodRequirement(string[] allowedRoles, string[] allowedMethods)
    {
        AllowedRoles = allowedRoles;
        AllowedMethods = allowedMethods;
    }
}