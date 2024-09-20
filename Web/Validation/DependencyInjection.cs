using System.Reflection;
using Application.DTO.House.Request;
using Application.DTO.HouseWeekInfo.Request;
using Application.DTO.Post.Request;
using Application.DTO.WeekMark.Request;
using FluentValidation;
using FluentValidation.AspNetCore;
using Web.Validation.House;
using Web.Validation.HouseWeekInfo;
using Web.Validation.Post;
using Web.Validation.WeekMark;

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

        services.AddTransient<IValidator<CreateWeekMarkRequest>, WeekMarkValidator>();
        services.AddTransient<IValidator<UpdateWeekMarkRequest>, WeekMarkValidator>();

        return services;
    }
}