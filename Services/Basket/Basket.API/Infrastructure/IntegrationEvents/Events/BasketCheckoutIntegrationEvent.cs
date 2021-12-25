using Basket.API.Models;
using IntegrationServices.Models;

namespace Basket.API.Infrastructure.IntegrationEvents.Events;

public record BasketCheckoutIntegrationEvent : IntegrationEvent
{
    public BasketCheckoutIntegrationEvent(string requestId, CustomerBasket basket)
    {
        RequestId = requestId;
        Basket = basket;
    }

    public string RequestId { get; }

    public CustomerBasket Basket { get; }
}
