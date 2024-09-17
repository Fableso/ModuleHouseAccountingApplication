using Web.ModelBinders;

namespace Web.Extensions;

public static class ModelBindersConfigurationExtension
{
    public static IMvcBuilder AddCustomModelBinders(this IMvcBuilder builder)
    {
        builder.AddMvcOptions(options =>
        {
            options.ModelBinderProviders.Insert(0, new StronglyTypedIdBinderProvider());
        });

        return builder;
    }
}