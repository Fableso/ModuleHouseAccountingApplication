using System.Reflection;
using Application.DTO.House.Request;
using Application.DTO.HouseWeekInfo.Request;
using Application.DTO.HouseWeekInfo.Response;
using Application.DTO.Post.Request;
using FluentValidation;
using FluentValidation.AspNetCore;
using Web.Validation.House;
using Web.Validation.HouseWeekInfo;
using Web.Validation.Post;

namespace Web.Validation;

public static class DependencyInjection
{
    public static IServiceCollection AddValidation(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddFluentValidationAutoValidation();
        
        services.AddTransient<IValidator<CreatePostRequest>, PostRequestValidator>();
        services.AddTransient<IValidator<UpdatePostRequest>, PostRequestValidator>();
        
        services.AddTransient<IValidator<CreateHouseRequest>, HouseRequestValidator>();

        services.AddTransient<IValidator<CreateHouseWeekInfoRequest>, CreateHouseInfoRequestValidator>();
        services.AddTransient<IValidator<UpdateHouseWeekInfoRequest>, HouseWeekInfoRequestValidator>();

        return services;
    }
}