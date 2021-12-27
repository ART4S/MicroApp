using EventBus.Abstractions;
using Microsoft.AspNetCore.SignalR;
using Ordering.SignalR.IntegrationEvents.Events;
using Ordering.SignalR.IntegrationEvents.Models;

namespace Ordering.SignalR.IntegrationEvents.EventHandlers;

public class OrderStatusChangedIntegrationEventHandler :
    IEventHandler<OrderCreatedIntegrationEvent>,
    IEventHandler<OrderConfirmedIntegrationEvent>,
    IEventHandler<OrderAcceptedIntegrationEvent>,
    IEventHandler<OrderPaidIntegrationEvent>,
    IEventHandler<OrderCancelledIntegrationEvent>
{
    private readonly IHubContext<NotificationsHub> _channel;

    public OrderStatusChangedIntegrationEventHandler(IHubContext<NotificationsHub> channel)
    {
        _channel = channel;
    }

    public Task Handle(OrderAcceptedIntegrationEvent @event)
    {
        return NotifyStatusChanged(@event.Order);
    }

    public Task Handle(OrderCreatedIntegrationEvent @event)
    {
        return NotifyStatusChanged(@event.Order);
    }

    public Task Handle(OrderConfirmedIntegrationEvent @event)
    {
        return NotifyStatusChanged(@event.Order);
    }

    public Task Handle(OrderPaidIntegrationEvent @event)
    {
        return NotifyStatusChanged(@event.Order);
    }

    public Task Handle(OrderCancelledIntegrationEvent @event)
    {
        return NotifyStatusChanged(@event.Order);
    }

    private Task NotifyStatusChanged(Order order)
    {
        return _channel.Clients
            .Groups(order.BuyerId.ToString())
            .SendAsync("UpdateOrderStatus", order);
    }
}