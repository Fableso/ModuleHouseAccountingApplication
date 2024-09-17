using Application.Abstractions;
using Application.DTO.House.Request;
using Domain.ValueObjects;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Web.Validation.Extensions;

namespace Web.Validation.House;

public class CreateHouseRequestValidator : AbstractValidator<CreateHouseRequest>
{
    public CreateHouseRequestValidator(IApplicationDbContext context)
    {
        RuleFor(x => new { x.Length, x.Width })
            .MustBeValidValueObject(pair => HouseMetrics.Create(pair.Length, pair.Width));

        RuleFor(x => new { StartDate = x.OfficialStartDate, EndDate = x.OfficialEndDate })
            .MustBeValidValueObject(pair => DateSpan.Create(pair.StartDate, pair.EndDate));
        
        RuleFor(x => x.Brigade).MustBeValidValueObject(Brigade.Create);
        
        RuleFor(x => x.Model).MustAsync(async (houseId, token) =>
        {
            return !await context.Houses.AnyAsync(house => house.Id == houseId, token);
        }).WithMessage("House with the provided model already exists. House model must be unique");
        
        RuleFor(x => x.PostIds)
            .NotEmpty()
            .WithMessage("The house must stand on at least one post")
            .MustBeUniqueCollection()
            .WithMessage("The house cannot stand on the same post more than once");
    }
}
