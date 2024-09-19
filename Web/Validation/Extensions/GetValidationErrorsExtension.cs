using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Web.Validation.Extensions;

public static class GetValidationErrorsExtension
{
    public static IEnumerable<string> GetValidationErrors(this ModelStateDictionary modelState)
    {
        var errors = modelState.Values
            .SelectMany(v => v.Errors)
            .Select(v => v.ErrorMessage);

        return errors;
    }
}