namespace Ordering.Domian.Abstractions;

public abstract class BaseEntity : IComparable
{
    public Guid Id { get; set; }

    public int CompareTo(object other) =>
        Equals(other) ? 1 : -1;

    public override bool Equals(object obj) =>
        obj is BaseEntity other &&
        GetType() == other.GetType() &&
        Id == other.Id;
}
