using Domain;
using Domain.ValueObjects;

namespace ModuleHouseAccountingApplication.Domain.Tests;

public class PostNameTests
{
    [Fact]
    public void PostNameCreate_TooLongPostName_ReturnsResultFail()
    {
        var postName = new string('a', DomainConstants.MaxPostNameLength + 1);
        var result = PostName.Create(postName);
        Assert.True(result.IsFailed);
    }
    
    [Fact]
    public void PostNameCreate_EmptyString_ReturnsResultFail()
    {
        var result = PostName.Create(string.Empty);
        Assert.True(result.IsFailed);
    }
    
    [Fact]
    public void PostNameCreate_WhiteSpaceString_ReturnsResultFail()
    {
        var result = PostName.Create("          ");
        Assert.True(result.IsFailed);
    }
    
    [Fact]
    public void PostNameCreate_CorrectData_ReturnsResultSuccess()
    {
        var postName = new string('a', DomainConstants.MaxPostNameLength);
        var result = PostName.Create(postName);
        Assert.True(result.IsSuccess);
        Assert.Equal(postName, result.Value.Value);
    }
}