using EventBus.RabbitMQ;
using EventBus.RabbitMQ.DependencyInjection;
using Payment.API.IntegrationEvents.EventHandlers;

namespace Payment.API.Configuration;

static class ServicesConfiguration
{
    public static void AddIntegrationServices(this IServiceCollection services, IConfiguration configuration)
    {
        RabbitMQSettings settings = new();
        configuration.GetSection("RabbitMQSettings").Bind(settings);
        services.AddRabbitMQEventBus(settings);
    }

    public static void AddEventHandlers(this IServiceCollection services)
    {
        services.AddScoped<OrderAcceptedIntegrationEventHandler>();
    }
}