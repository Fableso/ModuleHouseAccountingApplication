using Application.Abstractions;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<MhDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultModuleHouseMSSQLConnection")));

        services.AddScoped<IApplicationDbContext>(provider =>
            provider.GetRequiredService<MhDbContext>());

        return services;
    }
}
