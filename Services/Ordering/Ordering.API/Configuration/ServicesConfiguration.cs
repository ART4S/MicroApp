using EventBus.RabbitMQ.DependencyInjection;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Ordering.Application.Integration.EventHandlers;
using Ordering.Application.PipelineBehaviours;
using Ordering.Application.Services.Common;
using Ordering.Application.Services.DataAccess;
using Ordering.Infrastructure.DataAccess.Ordering;
using Microsoft.EntityFrameworkCore;
using IntegrationServices;
using System.Reflection;
using TaskScheduling.Core;
using Ordering.API.Infrastructure.BackgroundTasks;
using System.Data.Common;
using Microsoft.Data.SqlClient;
using Ordering.Application.Services.Identity;
using Ordering.Infrastructure.Identity;
using IntegrationServices.EF;
using IdempotencyServices.EF;
using IdempotencyServices.Mediator;
using Ordering.Infrastructure.Common;
using IdempotencyServices;
using Ordering.Application.Requests.Orders.CreateOrder;
using Ordering.API.Infrastructure.Attributes;
using FluentValidation.AspNetCore;
using Ordering.Application.Requests.Orders.ConfirmOrder;

namespace Ordering.API.Configuration;

static class ServicesConfiguration
{
    private static Assembly ApplicationAssembly 
        => typeof(IOrderingDbContext).Assembly;

    private static Assembly InfrastructureAssembly
        => typeof(OrderingDbContext).Assembly;

    public static void AddAppServices(this IServiceCollection services)
    {
        services.AddSingleton<ICurrentTime, CurrentTime>();
    }

    public static void AddIdentityServices(this IServiceCollection services)
    {
        services.AddScoped<IIdentityService, IdentityService>();
    }

    public static void AddSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new()
            {
                Title = "MicroShop - Ordering.API",
                Version = "v1"
            });
        });
    }

    public static void AddOrderingDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        string connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddScoped<DbConnection>((sp) => new SqlConnection(connectionString));

        services.AddDbContext<OrderingDbContext>((sp, options) =>
        {
            options.UseSqlServer(
                connection: sp.GetRequiredService<DbConnection>(), 
                sqlOptions => sqlOptions.MigrationsAssembly(InfrastructureAssembly.FullName));
        });

        services.AddScoped<IOrderingDbContext, OrderingDbContext>();
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
                connection: sp.GetRequiredService<DbConnection>(),
                sqlOptions => sqlOptions.MigrationsAssembly(InfrastructureAssembly.FullName));
        });

        services.AddScoped<IIntegrationDbContext, IntegrationDbContext>();

        services.AddScoped<IIntegrationEventService, IntegrationEventService>(
            (sp) => ActivatorUtilities.CreateInstance<IntegrationEventService>(sp, ApplicationAssembly));
    }

    public static void AddEventHandlers(this IServiceCollection services)
    {
        services.AddScoped<BasketCheckoutIntegrationEventHandler>();
    }

    public static void AddMediator(this IServiceCollection services)
    {
        services.AddMediatR(ApplicationAssembly);

        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(SaveChangesBehaviour<,>));
    }

    public static void AddAutoMapper(this IServiceCollection services)
    {
        services.AddAutoMapper(config =>
        {
            config.AddMaps(ApplicationAssembly);
        });
    }

    public static void AddValidation(this IServiceCollection services)
    {
        services.AddMvc(opt =>
        {
            opt.Filters.Add<ValidationAttribute>();
        }).AddFluentValidation();

        services.AddValidatorsFromAssembly(ApplicationAssembly);
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

    public static void ConfigureApi(this IServiceCollection services)
    {
        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.SuppressModelStateInvalidFilter = true;
        });
    }

    public static void AddIdempotencyServices(this IServiceCollection services, IConfiguration configuration)
    {
        string connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<IdempotencyDbContext>(options =>
        {
            options.UseSqlServer(connectionString, sqlOptions =>
            {
                sqlOptions.MigrationsAssembly(InfrastructureAssembly.FullName);
            });
        });

        services.AddScoped<IIdempotencyDbContext, IdempotencyDbContext>();

        services.AddSingleton<Func<DateTime>>(sp =>
        {
            var currentTime = sp.GetRequiredService<ICurrentTime>();
            return () => currentTime.Now;
        });

        services.AddScoped<IClientRequestService, ClientRequestService>();

        services.AddScoped
        (
            typeof(IRequestHandler<IdempotentRequest<CreateOrderCommand, Unit>, Unit>),
            typeof(IdempotentRequestHandler<CreateOrderCommand, Unit>)
        );

        services.AddScoped
        (
            typeof(IRequestHandler<IdempotentRequest<ConfirmOrderCommand, Unit>, Unit>),
            typeof(IdempotentRequestHandler<ConfirmOrderCommand, Unit>)
        );
    }
}
