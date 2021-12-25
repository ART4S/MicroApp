using System.Text.Json.Serialization;

namespace Payment.API.IntegrationEvents.Models;

public record AcceptedOrder
{
    [JsonInclude]
    public Guid OrderId { get; private set; }

    [JsonInclude]
    public decimal Total { get; private set; }
}

public record BuyerCardInfo
{
    [JsonInclude]
    public string CardNumber { get; private set; }

    [JsonInclude]
    public string SecurityNumber { get; private set; }

    [JsonInclude]
    public string CardHolderName { get; private set; }

    [JsonInclude]
    public DateTime Expiration { get; private set; }

    [JsonInclude]
    public int CardTypeId { get; private set; }
}

