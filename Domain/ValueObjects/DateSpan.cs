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
        var errors = new List<string>();
        if (startDate < DomainConstants.MinDate)
        {
            errors.Add($"The StartDate must be later than {DomainConstants.MinDate}");
        }

        if (startDate > endDate)
        {
            errors.Add("The StartDate must be earlier than the EndDate");
        }
        
        return errors.Count != 0
            ? Result.Fail<DateSpan>(errors)
            : Result.Ok(new DateSpan(startDate, endDate));
    }

    protected override IEnumerable<object?> ComparisonProperties()
    {
        yield return StartDate;
        yield return EndDate;
    }
}