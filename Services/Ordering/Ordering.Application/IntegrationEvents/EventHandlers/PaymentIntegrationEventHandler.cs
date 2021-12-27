using EventBus.Abstractions;
using IntegrationServices.Models;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.IntegrationEvents.Events;
using Ordering.Application.Requests.Orders.CancelOrder;
using Ordering.Application.Requests.Orders.SetOrderStatusToPaid;

namespace Ordering.Application.IntegrationEvents.EventHandlers;

public class PaymentIntegrationEventHandler :
    IEventHandler<PaymentSucceedIntegrationEvent>,
    IEventHandler<PaymentFailedIntegrationEvent>
{
    private readonly ILogger _logger;
    private readonly IMediator _mediator;

    public PaymentIntegrationEventHandler(
        ILogger<PaymentIntegrationEventHandler> logger,
        IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    public Task Handle(PaymentSucceedIntegrationEvent @event)
    {
        return ProcessPaymentEvent(@event, () => _mediator.Send(new SetOrderStatusToPaidCommand(@event.OrderId)));
    }

    public Task Handle(PaymentFailedIntegrationEvent @event)
    {
        return ProcessPaymentEvent(@event, () => _mediator.Send(new CancelOrderCommand(@event.OrderId)));
    }

    private async Task ProcessPaymentEvent<TEvent>(TEvent @event, Func<Task> processingAction) 
        where TEvent : IntegrationEvent
    {
        _logger.LogInformation("Start processing event {@Event}", @event);

        try
        {
            await processingAction();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occured while processing event {@EventId}", @event.Id);
            return;
        }

        _logger.LogInformation("Processing event {@EventId} succeed", @event.Id);
    }
}