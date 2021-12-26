using EventBus.Abstractions;
using IdempotencyServices.Mediator;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.IntegrationEvents.Events;
using Ordering.Application.Requests.Orders.CreateOrder;

namespace Ordering.Application.IntegrationEvents.EventHandlers;

public class BasketCheckoutIntegrationEventHandler : IEventHandler<BasketCheckoutIntegrationEvent>
{
    private readonly ILogger _logger;
    private readonly IMediator _mediator;

    public BasketCheckoutIntegrationEventHandler(
        ILogger<BasketCheckoutIntegrationEventHandler> logger,
        IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    public async Task Handle(BasketCheckoutIntegrationEvent @event)
    {
        try
        {
            var command = new IdempotentRequest<CreateOrderCommand, Unit>
            (
                id: @event.RequestId,
                originalRequest: new(@event.UserId, @event.UserName, @event.Basket)
            );

            await _mediator.Send(command);
        }
        catch (Exception ex)
        {
            // TODO: log
            _logger.LogError("", ex);
        }
    }
}
