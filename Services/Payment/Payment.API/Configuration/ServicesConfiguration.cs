using EventBus.RabbitMQ.DependencyInjection;
using Payment.API.Integration.EventHandlers;

namespace Payment.API.Configuration;

static class ServicesConfiguration
{
    public static void AddIntegrationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddRabbitMQEventBus(settings: new
        (
            HostName: configuration.GetValue<string>("RabbitMQSettings:HostName"),
            Retries: configuration.GetValue<int>("RabbitMQSettings:Retries"),
            ClientName: configuration.GetValue<string>("RabbitMQSettings:ClientName"),
            UserName: configuration.GetValue<string>("RabbitMQSettings:UserName"),
            Password: configuration.GetValue<string>("RabbitMQSettings:Password")
        ));
    }

    public static void AddEventHandlers(this IServiceCollection services)
    {
        services.AddScoped<OrderAcceptedIntegrationEventHandler>();
    }
}