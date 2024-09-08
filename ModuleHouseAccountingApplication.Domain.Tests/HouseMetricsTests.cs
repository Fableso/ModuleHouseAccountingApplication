using Domain.ValueObjects;

namespace ModuleHouseAccountingApplication.Domain.Tests;

public class HouseMetricsTests
{
    [Fact]
    public void HouseMetricsCreate_CorrectData_ReturnsResultSuccess()
    {
        var length = 20.5;
        var width = 11.54;
        var result = HouseMetrics.Create(length, width);
        Assert.True(result.IsSuccess);
        Assert.Equal(length, result.Value.Length);
        Assert.Equal(width, result.Value.Width);
    }

    [Theory]
    [InlineData(-1, -1)]
    [InlineData(-25.3, 25.5)]
    [InlineData(15.4, 0)]
    [InlineData(0, 0)]
    [InlineData(15, -15)]
    public void HouseMetricsCreate_InvalidDate_ReturnsResultFail(double length, double width)
    {
        var result = HouseMetrics.Create(length, width);
        Assert.True(result.IsFailed);
    }

    [Theory]
    [InlineData(5, 4, 5, 4, true)]
    [InlineData(5, 4, 5, 3, false)]
    [InlineData(5, 4, 4, 4, false)]
    [InlineData(1, 2, 3, 4, false)]
    public void HouseComparison_CorrectlyComparesTwoObjects(double lengthFirst, double widthFirst, double lengthSecond, double widthSecond, bool expectedResult)
    {
        var left = HouseMetrics.Create(lengthFirst, widthFirst).Value;
        var right = HouseMetrics.Create(lengthSecond, widthSecond).Value;
        var comparison = left.Equals(right);
        Assert.Equal(expectedResult, comparison);
    }
}