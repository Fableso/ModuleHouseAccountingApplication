using Application.DTO.House.Request;
using FluentValidation;

namespace Web.Validation.House;

public class CreateHouseRequestValidator : AbstractValidator<CreateHouseRequest>
{
    public CreateHouseRequestValidator(IValidator<IHouseRequest> houseRequestValidator)
    {
        Include(houseRequestValidator);
    }
}