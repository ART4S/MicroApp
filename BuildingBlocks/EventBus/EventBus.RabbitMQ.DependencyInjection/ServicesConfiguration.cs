using EventBus.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace EventBus.RabbitMQ.DependencyInjection;

public static class ServicesConfiguration
{
    public static void AddRabbitMQEventBus(this IServiceCollection services, RabbitMQSettings settings)
    {
        services.AddSingleton<IEventBus>((sp) => ActivatorUtilities.CreateInstance<RabbitMQEventBus>(sp, settings));
    }
}