namespace Domain.Common;

public abstract class ValueObject
{
    public override bool Equals(object? obj)
    {
        if (obj == null || obj.GetType() != GetType())
        {
            return false;
        }

        var compareTo = (ValueObject)obj;
        return ComparisonProperties().SequenceEqual(compareTo.ComparisonProperties());
    }

    protected abstract IEnumerable<object?> ComparisonProperties();
    

    public override int GetHashCode()
    {
        var hash = new HashCode();

        foreach (var component in ComparisonProperties())
        {
            hash.Add(component);
        }

        return hash.ToHashCode();
    }
}