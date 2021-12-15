using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Web.API.Services.Catalog;
using Web.API.Settings;

namespace Web.API.Configuration;

static class ServicesConfiguration
{
    public static void AddAppServices(this IServiceCollection services)
    {

    }

    public static void AddCatalogService(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<CatalogUrls>(configuration.GetSection("ExternalUrls:Catalog"));

        services.AddHttpClient<ICatalogService, CatalogService>((sp, client) =>
        {
            var settings = sp.GetRequiredService<IOptions<CatalogUrls>>().Value;

            client.BaseAddress = new Uri(settings.BasePath);
        });
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
        //services.AddAutoMapper(config =>
        //{
        //    config.AddMaps(ApplicationAssembly);
        //});
    }

    public static void AddValidation(this IServiceCollection services)
    {
        //services.AddMvc(opt =>
        //{
        //    opt.Filters.Add<ValidationAttribute>();
        //}).AddFluentValidation();

        //services.AddValidatorsFromAssembly(ApplicationAssembly);
    }

    public static void ConfigureApi(this IServiceCollection services)
    {
        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.SuppressModelStateInvalidFilter = true;
        });
    }
}
