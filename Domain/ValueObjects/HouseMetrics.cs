using Domain.Common;
using FluentResults;

namespace Domain.ValueObjects;

public class HouseMetrics : ValueObject
{
    private HouseMetrics(double length, double width)
    {
        Length = length;
        Width = width;
    }
    public double Length { get; }
    
    public double Width { get; }

    public static Result<HouseMetrics> Create(double length, double width)
    {
        var errors = new List<string>();

        if (length <= 0)
        {
            errors.Add("Length must be bigger than 0.");
        }

        if (width <= 0)
        {
            errors.Add("Width must be bigger than 0.");
        }

        return errors.Count != 0 ?
            Result.Fail<HouseMetrics>(errors)
            : Result.Ok(new HouseMetrics(length, width));
    }

    protected override IEnumerable<object> ComparisonProperties()
    {
        yield return Length;
        yield return Width;
    }
}