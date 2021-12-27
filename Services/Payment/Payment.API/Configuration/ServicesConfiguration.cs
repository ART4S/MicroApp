using EventBus.RabbitMQ;
using EventBus.RabbitMQ.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
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

    public static void AddHealthChecks(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHealthChecks()
            .AddCheck(
                name: "Self",
                check: () => HealthCheckResult.Healthy(),
                tags: new[] { "api" })
            .AddRabbitMQ(
                rabbitConnectionString: configuration.GetValue<string>("RabbitMQSettings:Uri"),
                name: "MQ",
                tags: new[] { "rabbitmq" });
    }
}