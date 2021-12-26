using EventBus.Abstractions;
using EventBus.RabbitMQ;
using EventBus.RabbitMQ.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Ordering.SignalR;
using Ordering.SignalR.IntegrationEvents.EventHandlers;
using Ordering.SignalR.IntegrationEvents.Events;
using System.IdentityModel.Tokens.Jwt;

class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddSignalR();

        JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.Authority = Configuration["IdentityUrl"];
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new()
                {
                    ValidateAudience = false
                };
                options.Events = new()
                {
                    OnMessageReceived = context =>
                    {
                        var token = context.Request.Query["access_token"];
                        var path = context.HttpContext.Request.Path;
                        if (!string.IsNullOrEmpty(token) && path.StartsWithSegments("/notifications"))
                            context.Token = token;

                        return Task.CompletedTask;
                    }
                };
            });

        services.AddAuthorization(options =>
        {
            options.DefaultPolicy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .RequireClaim("sub")
                .RequireClaim("name")
                .Build();
        });

        RabbitMQSettings settings = new();
        Configuration.GetSection("RabbitMQSettings").Bind(settings);
        services.AddRabbitMQEventBus(settings);

        services.AddScoped<OrderStatusChangedIntegrationEventHandler>();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseFileServer();

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints => 
        {
            endpoints.MapHub<NotificationsHub>("/notifications");
        });

        app.SubscribeToEvents();
    }
}

static class AppConfiguration
{
    public static void SubscribeToEvents(this IApplicationBuilder app)
    {
        IEventBus eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();

        eventBus.Subscribe<OrderCreatedIntegrationEvent, OrderStatusChangedIntegrationEventHandler>();
        eventBus.Subscribe<OrderConfirmedIntegrationEvent, OrderStatusChangedIntegrationEventHandler>();
        eventBus.Subscribe<OrderAcceptedIntegrationEvent, OrderStatusChangedIntegrationEventHandler>();
        eventBus.Subscribe<OrderPaidIntegrationEvent, OrderStatusChangedIntegrationEventHandler>();
        eventBus.Subscribe<OrderCancelledIntegrationEvent, OrderStatusChangedIntegrationEventHandler>();
    }
}
