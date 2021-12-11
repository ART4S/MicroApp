using IntegrationServices.Model;
using Ordering.Application.Integration.Models;

namespace Ordering.Application.Integration.Events;

public record BasketCheckoutIntegrationEvent : IntegrationEvent
{
    public BasketCheckoutIntegrationEvent(Guid requestId, CustomerBasket basket)
    {
        RequestId = requestId;
        Basket = basket;
    }

    public Guid RequestId { get; private init; }

    public CustomerBasket Basket { get; private init; }
}
