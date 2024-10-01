using FluentValidation;
using FluentValidation.Validators;
using Microsoft.AspNetCore.Identity;

namespace Web.Validation.Auth;

public class PasswordValidator<T> : PropertyValidator<T, string>
{
    private readonly PasswordOptions _passwordOptions;
    public override string Name  => "PasswordValidator";

    public PasswordValidator(PasswordOptions options)
    {
        _passwordOptions = options;
    }
    
    protected override string GetDefaultMessageTemplate(string errorCode)
    {
        return "{PropertyName} is invalid. Errors: {ErrorMessages}";
    }
    
    public override bool IsValid(ValidationContext<T> context, string value)
    {
        var errors = new List<string>();

        if (string.IsNullOrEmpty(value))
        {
            errors.Add("Password cannot be empty.");
        }
        else
        {
            CheckPasswordLength(value, errors);
            CheckUppercaseRequirement(value, errors);
            CheckLowercaseRequirement(value, errors);
            CheckDigitRequirement(value, errors);
            CheckSpecialCharacterRequirement(value, errors);
        }

        if (errors.Count <= 0) return true;
        
        
        context.MessageFormatter.AppendArgument("ErrorMessages", string.Join("; ", errors));
        return false;

    }

    private void CheckPasswordLength(string value, List<string> errors)
    {
        if (value.Length < _passwordOptions.RequiredLength)
        {
            errors.Add($"Password must be at least {_passwordOptions.RequiredLength} characters long.");
        }
    }

    private void CheckUppercaseRequirement(string value, List<string> errors)
    {
        if (_passwordOptions.RequireUppercase && !value.Any(char.IsUpper))
        {
            errors.Add("Password must contain at least one uppercase letter.");
        }
    }

    private void CheckLowercaseRequirement(string value, List<string> errors)
    {
        if (_passwordOptions.RequireLowercase && !value.Any(char.IsLower))
        {
            errors.Add("Password must contain at least one lowercase letter.");
        }
    }

    private void CheckDigitRequirement(string value, List<string> errors)
    {
        if (_passwordOptions.RequireDigit && !value.Any(char.IsDigit))
        {
            errors.Add("Password must contain at least one digit.");
        }
    }

    private void CheckSpecialCharacterRequirement(string value, List<string> errors)
    {
        if (_passwordOptions.RequireNonAlphanumeric && value.All(char.IsLetterOrDigit))
        {
            errors.Add("Password must contain at least one special character.");
        }
    }

}