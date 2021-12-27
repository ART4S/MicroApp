namespace Ordering.Application.IntegrationEvents.Models;

public record AcceptedOrder(
    Guid OrderId, Guid BuyerId, 
    int OrderStatusId, decimal Total, 
    BuyerCardInfo PaymentCard);

public record BuyerCardInfo(
    string CardNumber, string SecurityNumber, 
    string CardHolderName, DateTime Expiration, 
    int CardTypeId);

