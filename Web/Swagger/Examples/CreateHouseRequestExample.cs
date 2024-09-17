using System.Globalization;
using Application.DTO.House.Request;
using Domain.StronglyTypedIds;
using Swashbuckle.AspNetCore.Filters;

namespace Web.Swagger.Examples;

public class CreateHouseRequestExample : IExamplesProvider<CreateHouseRequest>
{
    public CreateHouseRequest GetExamples()
    {
        return new CreateHouseRequest
        {
            Model = new HouseId("TestHouse"), // Using custom constructor for HouseId
            Length = 5,
            Width = 6,
            TopLeftCornerX = -4,
            TopLeftCornerY = 18,
            OfficialStartDate = DateOnly.Parse("2023-02-17", CultureInfo.InvariantCulture), // Correct DateOnly format
            OfficialEndDate = DateOnly.Parse("2023-06-01", CultureInfo.InvariantCulture),   // Correct DateOnly format
            Brigade = "TestBrigade",
            PostIds = [new PostId(1), new PostId(3)] // Using custom constructor for PostId
        };
    }
}