using Domain.Common;

namespace Domain.ValueObjects;

public class Point : ValueObject
{
    public Point(int x, int y)
    {
        X = x;
        Y = y;
    }
    public int X { get; }
    public int Y { get; }
    
    protected override IEnumerable<object?> ComparisonProperties()
    {
        yield return X;
        yield return Y;
    }
}