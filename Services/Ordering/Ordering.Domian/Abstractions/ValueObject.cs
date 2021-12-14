namespace Ordering.Domian.Abstractions;

public abstract class ValueObject
{
    protected static bool EqualOperator(ValueObject left, ValueObject right) =>
        !(ReferenceEquals(left, null) ^ ReferenceEquals(right, null)) &&
        (ReferenceEquals(left, null) || left.Equals(right));

    protected static bool NotEqualOperator(ValueObject left, ValueObject right) =>
        !EqualOperator(left, right);

    protected abstract IEnumerable<object> GetEqualityComponents();

    public override bool Equals(object obj) =>
        obj is ValueObject other &&
        GetType() == other.GetType() &&
        GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());

    public override int GetHashCode() =>
        GetEqualityComponents()
            .Select(x => x != null ? x.GetHashCode() : 0)
            .Aggregate((x, y) => x ^ y);
}
