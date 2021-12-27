using Catalog.API.Application.IntegrationEvents.EventHandlers;
using Catalog.API.Application.PipelineBehaviours;
using Catalog.API.DataAccess;
using Catalog.API.DataAccess.Repositories;
using Catalog.API.Infrastructure.Attributes;
using Catalog.API.Infrastructure.BackgroundTasks;
using Catalog.API.Settings;
using EventBus.RabbitMQ;
using EventBus.RabbitMQ.DependencyInjection;
using FluentValidation;
using FluentValidation.AspNetCore;
using IntegrationServices;
using IntegrationServices.Mongo;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using TaskScheduling.Core;
using TaskScheduling.DependencyInjection;

namespace Catalog.API.Configuration;

static class ServicesConfiguration
{
    public static void AddSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new()
            {
                Title = "MicroShop - Catalog.API",
                Version = "v1",
            });
        });
    }

    public static void AddCatalogDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<CatalogDbSettings>(configuration.GetSection("CatalogDbSettings"));

        services.AddScoped<IMongoDatabase>(sp =>
        {
            CatalogDbSettings settings = sp.GetRequiredService<IOptions<CatalogDbSettings>>().Value;
            MongoClient client = new(settings.ConnectionString);
            return client.GetDatabase(settings.DatabaseName);
        });

        services.AddScoped<ICatalogDbContext, CatalogDbContext>();
    }

    public static void AddIntegrationServices(this IServiceCollection services, IConfiguration configuration)
    {
        RabbitMQSettings rabbitSettings = new();
        configuration.GetSection("RabbitMQSettings").Bind(rabbitSettings);

        services.AddRabbitMQEventBus(rabbitSettings);

        services.AddScoped<IMongoIntegrationDbContext, MongoIntegrationDbContext>();

        services.AddScoped<IIntegrationEventService, MongoIntegrationEventService>(sp =>
        {
            var itegrationDb = sp.GetRequiredService<IMongoIntegrationDbContext>();
            return new(itegrationDb, typeof(Startup).Assembly);
        });
    }

    public static void AddEventHandlers(this IServiceCollection services)
    {
        services.AddScoped<CatalogItemRemovedIntegrationEventHandler>();
        services.AddScoped<OrderConfirmedIntegrationEventHandler>();
        services.AddScoped<OrderPaidIntegrationEventHandler>();
    }

    public static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IPictureRepository, PictureRepository>();
    }

    public static void AddMediator(this IServiceCollection services)
    {
        services.AddMediatR(typeof(Startup));

        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(CommandValidationBehaviour<,>));
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(SaveChangesBehaviour<,>));
    }

    public static void AddAutoMapper(this IServiceCollection services)
    {
        services.AddAutoMapper(config =>
        {
            config.AddMaps(typeof(Startup).Assembly);
        });
    }

    public static void AddValidation(this IServiceCollection services)
    {
        services.AddMvc(opt =>
        {
            opt.Filters.Add<ValidationAttribute>();
        }).AddFluentValidation();

        services.AddValidatorsFromAssembly(typeof(Startup).Assembly);
    }

    public static void ConfigureApi(this IServiceCollection services)
    {
        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.SuppressModelStateInvalidFilter = true;
        });
    }

    public static void AddTaskScheduling(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScheduler(
            settings: new SchedulerSettings
            ( 
                PollingIntervalSec: configuration.GetValue<int>("BackgroundTasks:PollingIntervalSec")
            ), 
            taskSettings: new[]
            {
                new BackgroundTaskSettings<PublishIntegrationEventsBackgroundTask>(
                    Schedule: configuration["BackgroundTasks:IntegrationEventSchedule"])
            },
            exceptionHandler: (exception, task, services) =>
            {
                var loggerFactory = services.GetRequiredService<ILoggerFactory>();
                var logger = loggerFactory.CreateLogger(task.GetType());

                logger.LogError(exception, "Error occured while executing task {TaskType}", task.GetType().Name);
            });
    }

    public static void AddHealthChecks(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHealthChecks()
            .AddCheck(
                name: "Self",
                check: () => HealthCheckResult.Healthy(),
                tags: new[] { "api" })
            .AddMongoDb(
                mongodbConnectionString: configuration["CatalogDbSettings:ConnectionString"],
                name: "Catalog Db",
                tags: new[] { "database", "mongo" })
            .AddRabbitMQ(
                rabbitConnectionString: configuration["RabbitMQSettings:Uri"],
                name: "MQ",
                tags: new[] { "rabbitmq" });
    }
}
