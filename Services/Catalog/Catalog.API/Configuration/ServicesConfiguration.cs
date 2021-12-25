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
using IntegrationServices.EF;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        services.AddScoped<ICatalogDbContext>(sp =>
        {
            CatalogDbSettings settings = sp.GetRequiredService<IOptions<CatalogDbSettings>>().Value;
            MongoClient client = new(settings.ConnectionString);
            IMongoDatabase db = client.GetDatabase(settings.DatabaseName);
            return new CatalogDbContext(db);
        });
    }

    public static void AddIntegrationServices(this IServiceCollection services, IConfiguration configuration)
    {
        RabbitMQSettings rabbitSettings = new();
        configuration.GetSection("RabbitMQSettings").Bind(rabbitSettings);

        services.AddRabbitMQEventBus(rabbitSettings);

        IntegrationDbSettings integrationSettings = new();
        configuration.GetSection("IntegrationDbSettings").Bind(integrationSettings);

        services.AddDbContext<EFIntegrationDbContext>((sp, options) =>
        {
            options.UseSqlServer(
                integrationSettings.ConnectionString, 
                sqlOptions => sqlOptions.MigrationsAssembly(typeof(Startup).Assembly.FullName));
        });

        services.AddScoped<IEFIntegrationDbContext, EFIntegrationDbContext>();

        services.AddScoped<IIntegrationEventService, EFIntegrationEventService>(
            (sp) => ActivatorUtilities.CreateInstance<EFIntegrationEventService>(sp, typeof(Startup).Assembly));
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
                    Schedule: configuration.GetValue<string>("BackgroundTasks:IntegrationEventSchedule"))
            },
            exceptionHandler: (exception, task, services) =>
            {
                var loggerFactory = services.GetRequiredService<ILoggerFactory>();
                var logger = loggerFactory.CreateLogger(task.GetType());

                logger.LogError(exception, "Error occured while executing task {TaskType}", task.GetType().Name);
            });
    }
}
