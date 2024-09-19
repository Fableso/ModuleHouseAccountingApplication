using Domain.Common;
using FluentResults;

namespace Domain.ValueObjects;

public class WeekStartDate : ValueObject
{
    private WeekStartDate(DateOnly value)
    {
        Value = value;
    }
    public DateOnly Value { get; }
    
    public static Result<WeekStartDate> Create(DateOnly weekStartDate)
    {
        if (weekStartDate < DomainConstants.MinDate)
        {
            return Result.Fail<WeekStartDate>($"Week start date must be greater than {DomainConstants.MinDate}");
        }
        return Result.Ok(new WeekStartDate(weekStartDate));
    }
    
    protected override IEnumerable<object?> ComparisonProperties()
    {
        yield return Value;
    }
}