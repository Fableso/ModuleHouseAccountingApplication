using System.Text;
using Application.Abstractions;
using Application.Exceptions;
using Application.Services;
using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Identity;
using Infrastructure.Identity.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<MhDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultModuleHouseMSSQLConnection")));

        services.AddScoped<IApplicationDbContext>(provider =>
            provider.GetRequiredService<MhDbContext>());
        
        services.AddHttpContextAccessor();

        services.AddIdentityCore<ApplicationUser>(options =>
            {
                options.Password.RequiredLength = 9;
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredUniqueChars = 5;
            })
        .AddRoles<IdentityRole>()
        .AddEntityFrameworkStores<MhDbContext>()
        .AddDefaultTokenProviders();

        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,

                    ValidIssuer = Environment.GetEnvironmentVariable("JWT_ISSUER") ??
                                  throw new ConfigurationException("JWT_ISSUER is not set"),

                    ValidAudience = Environment.GetEnvironmentVariable("JWT_AUDIENCE") ??
                                    throw new ConfigurationException("JWT_AUDIENCE is not set"),

                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("JWT_SECRET_KEY") ??
                        throw new ConfigurationException("JWT_SECRET_KEY is not set")))
                };
            });

        services.AddAuthorizationBuilder()
            .AddPolicy("SpectatorPolicy", policy =>
                policy.Requirements.Add(new RoleAndMethodRequirement(
                    ["Spectator", "DefaultUser", "Admin"],
                    [HttpMethods.Get])))
            .AddPolicy("DefaultUserPolicy", policy =>
                policy.Requirements.Add(new RoleAndMethodRequirement(
                    ["DefaultUser", "Admin"],
                    [
                        HttpMethods.Get,
                        HttpMethods.Post,
                        HttpMethods.Put,
                        HttpMethods.Patch,
                        HttpMethods.Delete
                    ])))
            .AddPolicy("AdminPolicy", policy =>
                policy.RequireRole("Admin"));

        services.AddSingleton<IAuthorizationHandler, RoleAndMethodHandler>();
        services.AddScoped<IAdminService, AdminService>();

        return services;
    }
}