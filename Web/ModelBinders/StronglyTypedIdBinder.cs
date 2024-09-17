using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;

namespace Web.ModelBinders;

public class StronglyTypedIdBinder : IModelBinder
{
    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        var valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

        if (valueProviderResult == ValueProviderResult.None)
        {
            bindingContext.Result = ModelBindingResult.Failed();
            return Task.CompletedTask;
        }

        var valueStr = valueProviderResult.FirstValue;
        var postIdType = bindingContext.ModelType;
        var valueProperty = postIdType.GetProperty("Value");

        if (valueProperty == null)
        {
            bindingContext.Result = ModelBindingResult.Failed();
            return Task.CompletedTask;
        }

        var valueType = valueProperty.PropertyType;
        try
        {
            var value = Convert.ChangeType(valueStr, valueType);
            var postId = Activator.CreateInstance(postIdType, value);
            bindingContext.Result = ModelBindingResult.Success(postId);
        }
        catch (Exception)
        {
            bindingContext.Result = ModelBindingResult.Failed();
        }

        return Task.CompletedTask;
    }
}

public class StronglyTypedIdBinderProvider : IModelBinderProvider
{
    public IModelBinder? GetBinder(ModelBinderProviderContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        var modelType = context.Metadata.ModelType;
        var valueProperty = modelType.GetProperty("Value");

        if (!modelType.IsValueType || valueProperty == null) return null;
        var constructor = modelType.GetConstructor([valueProperty.PropertyType]);
        return constructor != null ? new BinderTypeModelBinder(typeof(StronglyTypedIdBinder)) : null;
    }
}