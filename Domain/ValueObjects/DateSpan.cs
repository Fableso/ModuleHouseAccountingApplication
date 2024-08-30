using Domain.Common;
using FluentResults;

namespace Domain.ValueObjects;

public class DateSpan : ValueObject
{
    private DateSpan(DateOnly startDate, DateOnly? endDate)
    {
        StartDate = startDate;
        EndDate = endDate;
    }
    
    public DateOnly StartDate { get;}
    public DateOnly? EndDate { get; }

    public static Result<DateSpan> Create(DateOnly startDate, DateOnly? endDate)
    {
        return startDate > endDate ? Result.Fail<DateSpan>("The StartDate must be earlier than the EndDate") : Result.Ok(new DateSpan(startDate, endDate));
    }

    protected override IEnumerable<object?> ComparisonProperties()
    {
        yield return StartDate;
        yield return EndDate;
    }
}