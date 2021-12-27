namespace Payment.API.IntegrationEvents.Models;

public record AcceptedOrder(Guid OrderId, decimal Total);

public record BuyerCardInfo(
    string CardNumber, string SecurityNumber,
    string CardHolderName, DateTime Expiration, int CardTypeId);

