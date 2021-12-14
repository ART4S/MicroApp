using System.Reflection;

namespace Ordering.Domian.Abstractions;

public abstract class Dictionary : IComparable
{
    public int Id { get; set; }
    public string Name { get; set; }

    public int CompareTo(object other) => 
        Equals(other) ? 1 : -1;

    public override bool Equals(object obj) =>
        obj is Dictionary other &&
        GetType() == other.GetType() &&
        Id == other.Id;

    public override int GetHashCode() => Id.GetHashCode();

    public override string ToString() => Name;

    public static IEnumerable<T> GetValues<T>() where T : Dictionary =>
        typeof(T).GetFields(
            BindingFlags.Public |
            BindingFlags.Static |
            BindingFlags.DeclaredOnly)
        .Select(x => x.GetValue(null))
        .Cast<T>();
}
