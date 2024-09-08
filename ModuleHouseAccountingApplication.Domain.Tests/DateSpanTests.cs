using System.Globalization;
using Domain.ValueObjects;

namespace ModuleHouseAccountingApplication.Domain.Tests;

public class DateSpanTests
{
    [Fact]
    public void DateSpanCreate_StartDateLaterThanEndDate_ReturnsResultFail()
    {
        var startDate = new DateOnly(2024, 12, 1);
        var endDate = new DateOnly(2024, 11, 1);
        var result = DateSpan.Create(startDate, endDate);
        Assert.True(result.IsFailed);
    }

    [Fact]
    public void DateSpanCreate_CorrectData_ReturnsResultSuccess()
    {
        var startDate = new DateOnly(2024, 12, 1);
        var endDate = new DateOnly(2024, 12, 20);
        var result = DateSpan.Create(startDate, endDate);
        Assert.True(result.IsSuccess);
        Assert.Equal(startDate, result.Value.StartDate);
        Assert.Equal(endDate, result.Value.EndDate);
    }

    [Theory]
    [InlineData("2024-01-01", "2024-02-01",  "2024-01-01", "2024-03-01", false)]
    [InlineData("2024-01-01", "2024-02-01",  "2024-01-01", "2024-02-01", true)]
    [InlineData("2024-01-03", "2024-02-01",  "2024-01-01", "2024-02-01", false)]
    [InlineData("2024-01-01", null,  "2024-01-01", "2024-02-01", false)]
    [InlineData("2024-01-01", "2024-02-01",  "2024-01-01", null, false)]
    [InlineData("2024-01-01", null,  "2024-01-01", null, true)]
    public void DateSpanComparison_CorrectlyComparesTwoObjects(string startFirst, string? endFirst, string startSecond, string? endSecond, bool expectedResult)
    {
        DateOnly firstStart = DateOnly.Parse(startFirst, CultureInfo.InvariantCulture);
        DateOnly? firstEnd = endFirst is null ? null : DateOnly.Parse(endFirst, CultureInfo.InvariantCulture);
        DateOnly secondStart = DateOnly.Parse(startSecond, CultureInfo.InvariantCulture);
        DateOnly? secondEnd = endSecond is null ? null : DateOnly.Parse(endSecond, CultureInfo.InvariantCulture);

        var right = DateSpan.Create(firstStart, firstEnd).Value;
        var left = DateSpan.Create(secondStart, secondEnd).Value;

        var comparison = right.Equals(left);
        
        Assert.Equal(expectedResult, comparison);
    }
}