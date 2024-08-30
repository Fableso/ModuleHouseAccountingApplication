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
        if (length <= 0)
        {
            return Result.Fail<HouseMetrics>("Length must be bigger than 0");
        }

        if (width <= 0)
        {
            return Result.Fail<HouseMetrics>("Width must be bigger than 0");
        }

        return Result.Ok(new HouseMetrics(length, width));
    }

    protected override IEnumerable<object> ComparisonProperties()
    {
        yield return Length;
        yield return Width;
    }
}