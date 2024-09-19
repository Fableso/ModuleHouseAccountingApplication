using Application.DTO.HouseWeekInfo.Request;
using Domain.ValueObjects;
using FluentValidation;
using Web.Validation.Extensions;

namespace Web.Validation.HouseWeekInfo;

public class CreateHouseInfoRequestValidator : AbstractValidator<CreateHouseWeekInfoRequest>
{
    public CreateHouseInfoRequestValidator(IValidator<IHouseWeekInfoRequest> houseWeekInfoRequestValidator)
    {
        Include(houseWeekInfoRequestValidator);
        RuleFor(r => r.StartDate).MustBeValidValueObject(WeekStartDate.Create);
    }
}