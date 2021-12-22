using Google.Protobuf.Collections;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using Web.API.Configuration.Factories;
using Web.API.Mapper.Converters;
using Web.API.Services;
using Web.API.Services.Basket;
using Web.API.Services.Catalog;
using Web.API.Services.Identity;
using Web.API.Services.Ordering;
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
                string headerValue = httpContextAccessor.HttpContext.Request.Headers.Authorization[0];
                if (headerValue != null)
                    client.DefaultRequestHeaders.Authorization = new("Bearer", headerValue.Split(' ').Last());
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
            options.SwaggerDoc("Web.API", new() { Title = "MicroShop - Web.API" });
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
    }

    public static void ConfigureApi(this IServiceCollection services)
    {
        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.SuppressModelStateInvalidFilter = true;
        });
    }
}