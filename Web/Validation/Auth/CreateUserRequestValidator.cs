using Application.DTO.Auth;
using Application.DTO.Identity;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Web.Validation.Extensions;

namespace Web.Validation.Auth;

public class CreateUserRequestValidator : AbstractValidator<CreateUserRequest>
{
    
    public CreateUserRequestValidator(IOptions<IdentityOptions> identityOptions)
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Email is not valid");

        RuleFor(x => x.Password)
            .Password(identityOptions.Value.Password);
    }
}