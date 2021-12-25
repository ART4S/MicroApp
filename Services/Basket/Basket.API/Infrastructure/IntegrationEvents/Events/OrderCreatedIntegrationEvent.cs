using Basket.API.Infrastructure.IntegrationEvents.Models;
using IntegrationServices.Models;
using System.Text.Json.Serialization;

namespace Basket.API.Infrastructure.IntegrationEvents.Events;

public record OrderCreatedIntegrationEvent : IntegrationEvent
{
    [JsonInclude]
    public CreatedOrder Order { get; set; }
}
