using IntegrationServices.Model;
using Ordering.Application.Integration.Models;
using System.Text.Json.Serialization;

namespace Ordering.Application.Integration.Events;

public record BasketCheckoutIntegrationEvent : IntegrationEvent
{
    [JsonInclude]
    public Guid RequestId { get; private set; }

    [JsonInclude]
    public CustomerBasket Basket { get; private set; }
}
