using Basket.API.Model;
using IntegrationServices.Model;

namespace Basket.API.Infrastructure.Integration.Events;

public record BasketCheckoutIntegrationEvent : IntegrationEvent
{
    public BasketCheckoutIntegrationEvent(Guid requestId, CustomerBasket basket)
    {
        RequestId = requestId;
        Basket = basket;
    }

    public Guid RequestId { get; }

    public CustomerBasket Basket { get; }
}
