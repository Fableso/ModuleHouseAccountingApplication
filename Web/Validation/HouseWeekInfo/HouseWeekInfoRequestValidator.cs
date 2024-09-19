using Application.DTO.HouseWeekInfo.Request;
using FluentValidation;

namespace Web.Validation.HouseWeekInfo;

public class HouseWeekInfoRequestValidator : AbstractValidator<IHouseWeekInfoRequest>
{
    public HouseWeekInfoRequestValidator()
    {
        RuleFor(x => x.Status).IsInEnum();
        RuleFor(x => x.OnTime).NotNull();
    }
}