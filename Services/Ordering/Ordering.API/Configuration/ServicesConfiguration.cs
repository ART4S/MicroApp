using EventBus.RabbitMQ.DependencyInjection;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Ordering.Application.PipelineBehaviours;
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
using Ordering.API.Infrastructure.Attributes;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Ordering.API.Services;
using Ordering.API.Utils;
using Ordering.Application.Services;
using TaskScheduling.DependencyInjection;
using Ordering.Application.IntegrationEvents.EventHandlers;
using EventBus.RabbitMQ;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;

namespace Ordering.API.Configuration;

static class ServicesConfiguration
{
    public static void AddAppServices(this IServiceCollection services)
    {
        services.AddSingleton<ICurrentTime, CurrentTime>();
        services.AddSingleton<Func<DateTime>>(sp =>
        {
            var currentTime = sp.GetRequiredService<ICurrentTime>();
            return () => currentTime.Now;
        });

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
            options.SwaggerDoc("v1", new() 
            { 
                Title = "MicroShop - Ordering.API",
                Version = "v1",
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
        RabbitMQSettings settings = new();
        configuration.GetSection("RabbitMQSettings").Bind(settings);
        services.AddRabbitMQEventBus(settings);

        services.AddDbContext<EFIntegrationDbContext>((sp, options) =>
        {
            options.UseSqlServer(
                connection: sp.GetRequiredService<DbConnection>(),
                sqlOptions => sqlOptions.MigrationsAssembly(ReflectionInfo.InfrastructureAssembly.FullName));
        });

        services.AddScoped<IEFIntegrationDbContext, EFIntegrationDbContext>();

        services.AddScoped<IIntegrationEventService>(
            (sp) =>
            {
                return new EFIntegrationEventService(
                    integrationDb: sp.GetRequiredService<IEFIntegrationDbContext>(), 
                    integrationEventsAssembly: ReflectionInfo.ApplicationAssembly);
            });
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

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.Authority = configuration["IdentityUrl"];
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new()
                {
                    ValidateAudience = false
                };
            });
    }

    public static void AddCustomAuthorization(this IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            options.DefaultPolicy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .RequireClaim("sub")
                .RequireClaim("name")
                .Build();
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

        services.AddDbContext<EFIdempotencyDbContext>(options =>
        {
            options.UseSqlServer(
                connectionString, 
                sqlOptions => sqlOptions.MigrationsAssembly(ReflectionInfo.InfrastructureAssembly.FullName));
        });

        services.AddScoped<IEFIdempotencyDbContext, EFIdempotencyDbContext>();

        services.AddScoped<IClientRequestService, EFClientRequestService>();

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

    public static void AddHealthChecks(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHealthChecks()
            .AddCheck(
                name: "self", 
                check: () => HealthCheckResult.Healthy())
            .AddSqlServer(
                connectionString: configuration.GetConnectionString("DefaultConnection"), 
                name: "Database", 
                tags: new[] { "db", "sql" })
            .AddRabbitMQ(
                rabbitConnectionString: configuration.GetValue<string>("RabbitMQSettings:Uri"), 
                name: "RabbitMQ", 
                tags: new [] { "rabbitmq" });
    }
}
