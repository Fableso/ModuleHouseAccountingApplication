using Application.Abstractions;
using Application.Mappers;
using Application.Services;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var mapperConfig = new MapperConfiguration(mc =>
        {
            mc.AddProfile(new AutomapperProfile());
        });
        services.AddSingleton(mapperConfig.CreateMapper());

        services.AddScoped<IHouseService, HouseService>();
        services.AddScoped<IHousePostService, HousePostService>();

        return services;
    }
}