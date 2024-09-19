using Application.DTO.Post.Request;
using Domain.ValueObjects;
using FluentValidation;
using Web.Validation.Extensions;

namespace Web.Validation.Post;

public class PostRequestValidator : AbstractValidator<IPostRequest>
{
    public PostRequestValidator()
    {
        RuleFor(x => x.Name)
            .MustBeValidValueObject(PostName.Create);

        RuleFor(x => x.Area).GreaterThan(0);
    }
}