using Application.Abstractions;
using Application.DTO.HouseWeekInfo.Response;
using Application.Mappers;
using Application.Services;
using AutoMapper;
using Domain.Entities;
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
        services.AddScoped<IPostService, PostService>();
        services.AddScoped<IHouseWeekInfoService, HouseWeekInfoService>();
        services.AddScoped<IWeekMarkService, WeekMarkService>();
        services.AddScoped<IHistoryService, HistoryService>();
        services.AddTransient<IEmailSender, SendGridEmailSender>();

        return services;
    }
}