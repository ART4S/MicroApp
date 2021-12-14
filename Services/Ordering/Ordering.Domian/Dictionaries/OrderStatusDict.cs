using Ordering.Domian.Abstractions;

namespace Ordering.Domian.Dictionaries;

public class OrderStatusDict : Dictionary
{
    public static readonly OrderStatusDict Submitted = new() { Id = 1, Name = nameof(Submitted) };
    public static readonly OrderStatusDict ConfirmedByUser = new() { Id = 2, Name = nameof(ConfirmedByUser) };
    public static readonly OrderStatusDict Accepted = new() { Id = 3, Name = nameof(Accepted) };
    public static readonly OrderStatusDict Paid = new() { Id = 4, Name = nameof(Paid) };
    public static readonly OrderStatusDict Shipped = new() { Id = 5, Name = nameof(Shipped) };
    public static readonly OrderStatusDict Cancelled = new() { Id = 6, Name = nameof(Cancelled) };
}
