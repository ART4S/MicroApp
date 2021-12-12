using EventBus.Abstractions;
using Ordering.API.Configuration.Middlewares;
using Ordering.Application.Integration.EventHandlers;
using Ordering.Application.Integration.Events;

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
    }
}