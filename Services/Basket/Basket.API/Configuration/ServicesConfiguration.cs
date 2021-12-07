using Basket.API.Infrastructure.DataAccess;
using Basket.API.Infrastructure.Mapper.Converters;
using EventBus.RabbitMQ.DependencyInjection;
using Google.Protobuf.Collections;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;

namespace Basket.API.Configuration;

static class ServicesConfiguration
{
    public static void ConfigureApi(this IServiceCollection services)
    {
        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.SuppressModelStateInvalidFilter = true;
        });
    }

    public static void AddDataAccess(this IServiceCollection services, IConfiguration configuration)
    {
        string connectionString = configuration.GetConnectionString("RedisConnection");

        services.AddSingleton<IConnectionMultiplexer>(_ => ConnectionMultiplexer.Connect(connectionString));

        services.AddScoped<IBasketRepository, BasketRepository>();
    }

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

    public static void AddAutoMapper(this IServiceCollection services)
    {
        services.AddAutoMapper(config => 
        {
            config.AddMaps(typeof(Startup));

            config.CreateMap(typeof(RepeatedField<>), typeof(List<>))
                .ConvertUsing(typeof(RepeatedFieldToListTypeConverter<,>));

            config.CreateMap(typeof(List<>), typeof(RepeatedField<>))
                .ConvertUsing(typeof(ListToRepeatedFieldTypeConverter<,>));
        });

        services.AddSingleton(typeof(RepeatedFieldToListTypeConverter<,>));
        services.AddSingleton(typeof(ListToRepeatedFieldTypeConverter<,>));
    }
}
