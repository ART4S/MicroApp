using Basket.API.Model;
using IntegrationServices.Model;

namespace Basket.API.Infrastructure.Integration.Events;

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
