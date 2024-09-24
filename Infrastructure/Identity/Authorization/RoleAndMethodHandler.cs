using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Identity.Authorization;

public class RoleAndMethodHandler : AuthorizationHandler<RoleAndMethodRequirement>
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public RoleAndMethodHandler(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, RoleAndMethodRequirement requirement)
    {
        var httpContext = _httpContextAccessor.HttpContext;
        var user = context.User;
        var method = httpContext?.Request.Method;

        if (!requirement.AllowedMethods.Contains(method))
        {
            context.Fail();
            return Task.CompletedTask;
        }

        if (Array.Exists(requirement.AllowedRoles, role => user.IsInRole(role)))
        {
            context.Succeed(requirement);
        }
        else
        {
            context.Fail();
        }

        return Task.CompletedTask;
    }
}