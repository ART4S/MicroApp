using Ordering.Domian.Abstractions;

namespace Ordering.Domian.Dictionaries;

public class CardTypeDict : Dictionary
{
    public static readonly CardTypeDict Visa = new() { Id = 1, Name = nameof(Visa) };
    public static readonly CardTypeDict Mastercard = new() { Id = 2, Name = nameof(Mastercard) };
}