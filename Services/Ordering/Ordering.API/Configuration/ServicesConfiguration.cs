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
using TaskScheduling.Core;
using Ordering.API.Infrastructure.BackgroundTasks;
using System.Data.Common;
using Microsoft.Data.SqlClient;
using IntegrationServices.EF;
using IdempotencyServices.EF;
using IdempotencyServices.Mediator;
using Ordering.Infrastructure.Common;
using IdempotencyServices;
using Ordering.Application.Requests.Orders.CreateOrder;
using Ordering.API.Infrastructure.Attributes;
using FluentValidation.AspNetCore;
using Ordering.Application.Requests.Orders.ConfirmOrder;
using System.IdentityModel.Tokens.Jwt;
using Ordering.API.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Ordering.API.Services;
using Ordering.API.Utils;

namespace Ordering.API.Configuration;

static class ServicesConfiguration
{
    public static void AddAppServices(this IServiceCollection services)
    {
        services.AddSingleton<ICurrentTime, CurrentTime>();
        services.AddScoped<IBuyerService, BuyerService>();
    }

    public static void AddIdentityServices(this IServiceCollection services)
    {
        services.AddScoped<IBuyerService, BuyerService>();
    }

    public static void AddSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("Ordering.API", new() 
            { 
                Title = "MicroShop - Ordering.API" 
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
                sqlOptions => sqlOptions.MigrationsAssembly(ReflectionInfo.InfrastructureAssembly.FullName));
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
                sqlOptions => sqlOptions.MigrationsAssembly(ReflectionInfo.InfrastructureAssembly.FullName));
        });

        services.AddScoped<IIntegrationDbContext, IntegrationDbContext>();

        services.AddScoped<IIntegrationEventService, IntegrationEventService>(
            (sp) => ActivatorUtilities.CreateInstance<IntegrationEventService>(sp, ReflectionInfo.ApplicationAssembly));
    }

    public static void AddEventHandlers(this IServiceCollection services)
    {
        services.AddScoped<BasketCheckoutIntegrationEventHandler>();
        services.AddScoped<OrderInStockCheckedIntegrationEventHandler>();
        services.AddScoped<PaymentIntegrationEventHandler>();
    }

    public static void AddMediator(this IServiceCollection services)
    {
        services.AddMediatR(ReflectionInfo.ApplicationAssembly);

        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(SaveChangesBehaviour<,>));
    }

    public static void AddAutoMapper(this IServiceCollection services)
    {
        services.AddAutoMapper(config =>
        {
            config.AddMaps(ReflectionInfo.ApplicationAssembly);
        });
    }

    public static void AddValidation(this IServiceCollection services)
    {
        services.AddMvc(opt =>
        {
            opt.Filters.Add<ValidationAttribute>();
        }).AddFluentValidation();

        services.AddValidatorsFromAssembly(ReflectionInfo.ApplicationAssembly);
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

    public static void AddCustomAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

        IdentityUrls settings = new();

        configuration.GetSection("ExternalUrls:Identity").Bind(settings);

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.Authority = settings.BasePath;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new()
                {
                    ValidateAudience = false
                };
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
            options.UseSqlServer(
                connectionString, 
                sqlOptions => sqlOptions.MigrationsAssembly(ReflectionInfo.InfrastructureAssembly.FullName));
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
            typeof(IRequestHandler<IdempotentRequest<Application.Requests.Orders.CreateOrder.CreateOrderCommand, Unit>, Unit>),
            typeof(IdempotentRequestHandler<Application.Requests.Orders.CreateOrder.CreateOrderCommand, Unit>)
        );

        services.AddScoped
        (
            typeof(IRequestHandler<IdempotentRequest<Application.Requests.Orders.ConfirmOrder.ConfirmOrderCommand, Unit>, Unit>),
            typeof(IdempotentRequestHandler<Application.Requests.Orders.ConfirmOrder.ConfirmOrderCommand, Unit>)
        );
    }

    public static void AddBuyerService(this IServiceCollection services)
    {
        services.AddScoped<IBuyerService, BuyerService>();
        services.AddHttpContextAccessor();
    }
}
