using Domain.Common;
using FluentResults;

namespace Domain.ValueObjects;

public class Brigade : ValueObject
{
    private Brigade(string value)
    {
        Value = value;
    }
    
    public string Value { get; }

    public static Result<Brigade> Create(string comment)
    {
        var trimmedBrigade = comment.Trim();
        return trimmedBrigade.Length > DomainConstants.MaxBrigadeNameLength
            ? Result.Fail($"Brigade name must be shorter than {DomainConstants.MaxBrigadeNameLength} symbols")
            : Result.Ok(new Brigade(trimmedBrigade));
    }

    protected override IEnumerable<object> ComparisonProperties()
    {
        yield return Value;
    }
}