using Application.Abstractions;
using Application.DTO.House.Request;
using Domain.ValueObjects;
using FluentValidation;
using Web.Validation.Extensions;

namespace Web.Validation.House;

public class UpdateHouseRequestValidator : AbstractValidator<UpdateHouseRequest>
{
    public UpdateHouseRequestValidator(IValidator<IHouseRequest> houseRequestValidator)
    {
        Include(houseRequestValidator);
        
        RuleFor(x => x.CurrentState)
            .IsInEnum();
        
        RuleFor(x => new { StartDate = x.RealStartDate, EndDate = x.RealEndDate })
            .MustBeValidValueObject(pair => DateSpan.Create(pair.StartDate, pair.EndDate));
    }
}