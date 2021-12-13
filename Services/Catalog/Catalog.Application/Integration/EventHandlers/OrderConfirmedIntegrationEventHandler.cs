using Catalog.Application.Integration.Events;
using Catalog.Application.Services.DataAccess;
using EventBus.Abstractions;

namespace Catalog.Application.Integration.EventHandlers;

public class OrderConfirmedIntegrationEventHandler : IEventHandler<OrderConfirmedIntegrationEvent>
{
    public OrderConfirmedIntegrationEventHandler()
    {

    }

    public Task Handle(OrderConfirmedIntegrationEvent @event)
    {
        throw new NotImplementedException();
    }
}