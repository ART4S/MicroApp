using Google.Protobuf.Collections;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using Web.API.Configuration.Factories;
using Web.API.Exceptions;
using Web.API.Mapper.Converters;
using Web.API.Services;
using Web.API.Settings;

namespace Web.API.Configuration;

static class ServicesConfiguration
{
    public static void AddCatalogService(this IServiceCollection services, IConfiguration configuration)
    {
        CatalogUrls settings = new();
        configuration.GetSection("ExternalUrls:Catalog").Bind(settings);
        services.AddSingleton(settings);

        services.AddHttpClient<ICatalogService, CatalogService>((sp, client) =>
        {
            client.BaseAddress = new Uri(settings.BasePath);
        }).AddDefaultPolicies();
    }

    public static void AddBasketService(this IServiceCollection services, IConfiguration configuration)
    {
        BasketUrls settings = new();
        configuration.GetSection("ExternalUrls:Basket").Bind(settings);
        services.AddSingleton(settings);

        services.AddGrpcClient<GrpcBasket.Basket.BasketClient>(options =>
        {
            options.Address = new Uri(settings.BasePath);
        }).AddDefaultPolicies();

        services.AddScoped<IBasketService, BasketService>();
    }

    public static void AddOrderingService(this IServiceCollection services, IConfiguration configuration)
    {
        OrderingUrls settings = new();
        configuration.GetSection("ExternalUrls:Ordering").Bind(settings);
        services.AddSingleton(settings);

        services.AddHttpClient<IOrderingService, OrderingService>((services, client) =>
        {
            client.BaseAddress = new Uri(settings.BasePath);

            var httpContextAccessor = services.GetRequiredService<IHttpContextAccessor>();

            if (httpContextAccessor.HttpContext != null)
            {
                var autorizationHeaders = httpContextAccessor.HttpContext.Request.Headers.Authorization;
                if (autorizationHeaders.Count == 0 || !autorizationHeaders[0].StartsWith("Bearer"))
                    throw InvalidRequestException.BadRequest("Bearer token is missing");

                client.DefaultRequestHeaders.Authorization = new("Bearer", autorizationHeaders[0].Split(' ').Last());
            }

        }).AddDefaultPolicies();
    }

    public static void AddUserService(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IUserService, UserService>();
    }

    public static void AddSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new()
            {
                Title = "MicroShop - Web.API",
                Version = "v1",
            });
        });
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
                options.SaveToken = false;
                options.TokenValidationParameters = new()
                {
                    ValidateAudience = false
                };
            });

        services.AddAuthorization(options =>
        {
            options.DefaultPolicy = new AuthorizationPolicyBuilder()
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
}