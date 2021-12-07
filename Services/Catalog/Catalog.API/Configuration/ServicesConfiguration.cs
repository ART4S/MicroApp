using Catalog.API.Infrastructure.Attributes;
using Catalog.API.Infrastructure.BackgroundTasks;
using Catalog.Application.Integration.EventHandlers;
using Catalog.Application.Integration.Events;
using Catalog.Application.Interfaces.DataAccess;
using Catalog.Application.PipelineBehaviours;
using Catalog.Infrastructure.DataAccess.Catalog;
using Catalog.Infrastructure.DataAccess.Catalog.Repositories;
using EventBus.Abstractions;
using EventBus.RabbitMQ.DependencyInjection;
using FluentValidation;
using FluentValidation.AspNetCore;
using IntegrationServices;
using IntegrationServices.DataAccess;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using TaskScheduling.Core;

namespace Catalog.API.Configuration;

static class ServicesConfiguration
{
    public static void AddSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new()
            {
                Title = "MicroApp - Catalog.API",
                Version = "v1"
            });
        });
    }

    public static void AddCatalogDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        string connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddScoped(_ => new SqlConnection(connectionString));

        services.AddDbContext<CatalogDbContext>((sp, options) =>
        {
            options.UseSqlServer(
                connection: sp.GetRequiredService<SqlConnection>(),
                sqlOptions => sqlOptions.MigrationsAssembly(typeof(CatalogDbContext).Assembly.FullName));
        });

        services.AddScoped<ICatalogDbContext, CatalogDbContext>();
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

        services.AddDbContext<IntegrationDbContext>((sp, options) =>
        {
            options.UseSqlServer(
                connection: sp.GetRequiredService<SqlConnection>(), 
                sqlOptions => sqlOptions.MigrationsAssembly(typeof(CatalogDbContext).Assembly.FullName));
        });

        services.AddScoped<IIntegrationDbContext, IntegrationDbContext>();

        services.AddScoped<IIntegrationEventService, IntegrationEventService>(
            (sp) => ActivatorUtilities.CreateInstance<IntegrationEventService>(sp, typeof(ICatalogDbContext).Assembly));
    }

    public static void AddEventHandlers(this IServiceCollection services)
    {
        services.AddScoped<CatalogItemRemovedIntegrationEventHandler>();
    }

    public static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IItemPictureRepository, ItemPictureRepository>();
    }

    public static void AddMediator(this IServiceCollection services)
    {
        services.AddMediatR(typeof(ICatalogDbContext));

        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(SaveChangesBehaviour<,>));
    }

    public static void AddAutoMapper(this IServiceCollection services)
    {
        services.AddAutoMapper(config =>
        {
            config.AddMaps(typeof(ICatalogDbContext));
        });
    }

    public static void AddValidation(this IServiceCollection services)
    {
        services.AddMvc(opt =>
        {
            opt.Filters.Add<ValidationAttribute>();
        }).AddFluentValidation();

        services.AddValidatorsFromAssemblyContaining(typeof(ICatalogDbContext));
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
                new BackgroundTaskSettings<IntegrationEventBackgroundTask>(
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
