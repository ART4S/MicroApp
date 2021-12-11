using IntegrationServices.Model;

namespace Ordering.Application.Integration.Events;

public record OrderStartedIntegrationEvent : IntegrationEvent
{
    public OrderStartedIntegrationEvent(Guid buyerId, Guid orderId)
    {
        BuyerId = buyerId;
        OrderId = orderId;
    }

    public Guid OrderId { get; private set; }
    public Guid BuyerId { get; private set; }
}
