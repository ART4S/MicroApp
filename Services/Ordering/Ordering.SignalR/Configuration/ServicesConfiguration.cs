using EventBus.RabbitMQ;
using EventBus.RabbitMQ.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Ordering.SignalR.IntegrationEvents.EventHandlers;
using System.IdentityModel.Tokens.Jwt;

namespace Ordering.SignalR.Configuration;

static class ServicesConfiguration
{
    public static void AddRabbitMq(this IServiceCollection services, IConfiguration configuration)
    {
        RabbitMQSettings settings = new();
        configuration.GetSection("RabbitMQSettings").Bind(settings);
        services.AddRabbitMQEventBus(settings);

        services.AddScoped<OrderStatusChangedIntegrationEventHandler>();
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
}
