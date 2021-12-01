using EventBus.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EventBus.RabbitMQ.DependencyInjection;

public static class Extensions
{
    public static void AddRabbitMQEventBus(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton(new RabbitMQSettings
        (    
            HostName: configuration.GetValue<string>("RabbitMQSettings:HostName"),
            Retries: configuration.GetValue<int>("RabbitMQSettings:Retries"),
            ClientName: configuration.GetValue<string>("RabbitMQSettings:ClientName"),
            UserName: configuration.GetValue<string>("RabbitMQSettings:UserName"),
            Password: configuration.GetValue<string>("RabbitMQSettings:Password")
        ));

        services.AddSingleton<IEventBus, RabbitMQEventBus>();
    }
}