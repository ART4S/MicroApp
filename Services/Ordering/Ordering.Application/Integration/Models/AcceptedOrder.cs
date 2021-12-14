namespace Ordering.Application.Integration.Models;

public record AcceptedOrder
{
    public Guid OrderId { get; set; }
    public decimal Total { get; set; }
    public BuyerCardInfo PaymentCard { get; set; }
}

public record BuyerCardInfo
{
    public string CardNumber { get; set; }

    public string SecurityNumber { get; set; }

    public string CardHolderName { get; set; }

    public DateTime Expiration { get; set; }

    public int CardTypeId { get; set; }
}
