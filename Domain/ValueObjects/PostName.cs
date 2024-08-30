using Domain.Common;
using FluentResults;

namespace Domain.ValueObjects;

public class PostName : ValueObject
{
    private PostName(string value)
    {
        Value = value;
    }
    
    public string Value { get; }

    public static Result<PostName> Create(string comment)
    {
        var trimmedName = comment.Trim();
        return trimmedName.Length > DomainConstants.MaxPostNameLength
            ? Result.Fail($"Post name must be shorter than {DomainConstants.MaxPostNameLength} symbols")
            : Result.Ok(new PostName(trimmedName));
    }

    protected override IEnumerable<object> ComparisonProperties()
    {
        yield return Value;
    }
}