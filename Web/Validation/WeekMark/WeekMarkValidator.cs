using Application.DTO.WeekMark.Request;
using Domain.ValueObjects;
using FluentValidation;
using Web.Validation.Extensions;

namespace Web.Validation.WeekMark;

public class WeekMarkValidator : AbstractValidator<IWeekMarkRequest>
{
    public WeekMarkValidator()
    {
        RuleFor(x => x.MarkType)
            .IsInEnum();

        RuleFor(x => x.Comment).MustBeValidValueObject(MarkComment.Create!);
    }
}