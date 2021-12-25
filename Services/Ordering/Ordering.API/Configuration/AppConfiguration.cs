using EventBus.Abstractions;
using Ordering.API.Configuration.Middlewares;
using Ordering.Application.IntegrationEvents.EventHandlers;
using Ordering.Application.IntegrationEvents.Events;

namespace Ordering.API.Configuration;

static class AppConfiguration
{
    public static void UseCustomExceptionHandler(this IApplicationBuilder app)
    {
        app.UseMiddleware<CustomExceptionHandlerMiddleware>();
    }

    public static void SubscribeToEvents(this IApplicationBuilder app)
    {
        var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();

        eventBus.Subscribe<BasketCheckoutIntegrationEvent, BasketCheckoutIntegrationEventHandler>();
        eventBus.Subscribe<OrderInStockCheckedIntegrationEvent, OrderInStockCheckedIntegrationEventHandler>();
        eventBus.Subscribe<PaymentSucceedIntegrationEvent, PaymentIntegrationEventHandler>();
        eventBus.Subscribe<PaymentFailedIntegrationEvent, PaymentIntegrationEventHandler>();
    }
}