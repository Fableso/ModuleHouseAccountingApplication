using System.Text.Json.Serialization;
using Domain.StronglyTypedIds;
using Web.Converters;

namespace Web.Extensions;

public static class JsonConfigurationExtension
{
    public static IMvcBuilder AddJsonSerializationOptions(this IMvcBuilder builder)
    {
        builder.AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.Converters.Add(new StronglyTypedIdJsonConverter<string, HouseId>());
            options.JsonSerializerOptions.Converters.Add(new StronglyTypedIdJsonConverter<long, PostId>());
            options.JsonSerializerOptions.Converters.Add(new StronglyTypedIdJsonConverter<long, HousePostId>());
            options.JsonSerializerOptions.Converters.Add(new StronglyTypedIdJsonConverter<long, HouseWeekInfoId>());
            options.JsonSerializerOptions.Converters.Add(new StronglyTypedIdJsonConverter<Guid, WeekMarkId>());
            
            
            var enumConverter = new JsonStringEnumConverter();
            options.JsonSerializerOptions.Converters.Add(enumConverter);
        });

        return builder;
    }
}