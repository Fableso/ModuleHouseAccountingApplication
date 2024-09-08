using Domain.Common;
using FluentResults;

namespace Domain.ValueObjects;

public class MarkComment : ValueObject
{
    private MarkComment(string value)
    {
        Value = value;
    }
    
    public string Value { get; }

    public static Result<MarkComment> Create(string comment)
    {
        var trimmedComment = comment.Trim();
        if (string.IsNullOrEmpty(trimmedComment))
        {
            return Result.Fail("Comment must not be empty");
        }
        
        return trimmedComment.Length > DomainConstants.MaxWeekMarkCommentLength
            ? Result.Fail($"Comment must be shorter than {DomainConstants.MaxWeekMarkCommentLength} symbols")
            : Result.Ok(new MarkComment(trimmedComment));
    }

    protected override IEnumerable<object> ComparisonProperties()
    {
        yield return Value;
    }
}