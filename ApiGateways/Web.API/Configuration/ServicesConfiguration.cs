using Google.Protobuf.Collections;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
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
        services.Configure<CatalogUrls>(configuration.GetSection("ExternalUrls:Catalog"));

        services.AddHttpClient<ICatalogService, CatalogService>((sp, client) =>
        {
            var settings = sp.GetRequiredService<IOptions<CatalogUrls>>().Value;
            client.BaseAddress = new Uri(settings.BasePath);
        });
    }

    public static void AddBasketService(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<BasketUrls>(configuration.GetSection("ExternalUrls:Basket"));

        services.AddScoped<IBasketService, BasketService>();

        services.AddGrpcClient<GrpcBasket.Basket.BasketClient>((sp, options) =>
        {
            var settings = sp.GetRequiredService<IOptions<BasketUrls>>().Value;
            options.Address = new Uri(settings.BasePath);
        });
    }

    public static void AddOrderingService(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<OrderingUrls>(configuration.GetSection("ExternalUrls:Ordering"));

        services.AddHttpClient<IOrderingService, OrderingService>((sp, client) =>
        {
            var settings = sp.GetRequiredService<IOptions<OrderingUrls>>().Value;
            client.BaseAddress = new Uri(settings.BasePath);
        });
    }

    public static void AddIdentityService(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IIdentityService, IdentityService>();
    }

    public static void AddSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new()
            {
                Title = "MicroShop - Web.API",
                Version = "v1"
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

    public static void ConfigureApi(this IServiceCollection services)
    {
        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.SuppressModelStateInvalidFilter = true;
        });
    }
}
