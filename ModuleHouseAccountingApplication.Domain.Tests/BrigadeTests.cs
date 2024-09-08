using Domain;
using Domain.ValueObjects;

namespace ModuleHouseAccountingApplication.Domain.Tests;

public class BrigadeTests
{
    [Fact]
    public void BrigadeCreate_TooLongBrigadeName_ReturnsResultFail()
    {
        var brigadeName = new string('a', DomainConstants.MaxBrigadeNameLength + 1);
        var result = Brigade.Create(brigadeName);
        Assert.True(result.IsFailed);
    }
    
    [Fact]
    public void BrigadeCreate_EmptyString_ReturnsResultFail()
    {
        var result = Brigade.Create(string.Empty);
        Assert.True(result.IsFailed);
    }
    
    [Fact]
    public void BrigadeCreate_WhiteSpaceString_ReturnsResultFail()
    {
        var result = Brigade.Create("          ");
        Assert.True(result.IsFailed);
    }
    
    [Fact]
    public void BrigadeCreate_CorrectData_ReturnsResultSuccess()
    {
        var brigadeName = new string('a', DomainConstants.MaxBrigadeNameLength);
        var result = Brigade.Create(brigadeName);
        Assert.True(result.IsSuccess);
        Assert.Equal(brigadeName, result.Value.Value);
    }
}