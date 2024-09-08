using Domain;
using Domain.ValueObjects;

namespace ModuleHouseAccountingApplication.Domain.Tests;

public class MarkCommentTests
{
    [Fact]
    public void MarkCommentCreate_TooLongComment_ReturnsResultFail()
    {
        var comment = new string('a', DomainConstants.MaxWeekMarkCommentLength + 1);
        var result = MarkComment.Create(comment);
        Assert.True(result.IsFailed);
    }
    
    [Fact]
    public void MarkCommentCreate_EmptyString_ReturnsResultFail()
    {
        var result = MarkComment.Create(string.Empty);
        Assert.True(result.IsFailed);
    }
    
    [Fact]
    public void MarkCommentCreate_WhiteSpaceString_ReturnsResultFail()
    {
        var result = MarkComment.Create("          ");
        Assert.True(result.IsFailed);
    }
    
    [Fact]
    public void MarkCommentCreate_CorrectData_ReturnsResultSuccess()
    {
        var comment = new string('a', DomainConstants.MaxWeekMarkCommentLength);
        var result = MarkComment.Create(comment);
        Assert.True(result.IsSuccess);
        Assert.Equal(comment, result.Value.Value);
    }
}