using EventBus.Abstractions;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Integration.Events;
using Ordering.Application.Requests.Orders.CancelOrder;
using Ordering.Application.Requests.Orders.SetOrderStatusToPaid;

namespace Ordering.Application.Integration.EventHandlers;

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

    public async Task Handle(PaymentSucceedIntegrationEvent @event)
    {
        // TODO: log

        try
        {
            await _mediator.Send(new SetOrderStatusToPaidCommand(@event.OrderId));
        }
        catch(Exception ex)
        {
            HandlePaymentException(ex);
            return;
        }

        // TODO: log
    }

    public async Task Handle(PaymentFailedIntegrationEvent @event)
    {
        // TODO: log

        try
        {
            await _mediator.Send(new CancelOrderCommand(@event.OrderId));
        }
        catch (Exception ex)
        {
            HandlePaymentException(ex);
            return;
        }

        // TODO: log
    }

    private void HandlePaymentException(Exception ex)
    {
        // TODO: log
        _logger.LogError("", ex);
    }
}