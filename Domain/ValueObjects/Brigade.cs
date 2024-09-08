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

    public static Result<Brigade> Create(string brigadeName)
    {
        var trimmedBrigade = brigadeName.Trim();
        if (string.IsNullOrEmpty(trimmedBrigade))
        {
            return Result.Fail("Brigade name must not be empty");
        }
        
        return trimmedBrigade.Length > DomainConstants.MaxBrigadeNameLength
            ? Result.Fail($"Brigade name must be shorter than {DomainConstants.MaxBrigadeNameLength} symbols")
            : Result.Ok(new Brigade(trimmedBrigade));
    }

    protected override IEnumerable<object> ComparisonProperties()
    {
        yield return Value;
    }
}