using Ordering.Domian.Abstractions;
using Ordering.Domian.Dictionaries;

namespace Ordering.Domian.Entities;

public class PaymentMethod : BaseEntity
{
    public string? Alias { get; set; }

    public string CardNumber { get; set; }

    public string SecurityNumber { get; set; }

    public string CardHolderName { get; set; }

    public DateTime Expiration { get; set; }

    public Guid BuyerId { get; set; }
    public Buyer Buyer { get; set; }

    public int CardTypeId { get; set; }
    public CardTypeDict CardType { get; set; }
}
