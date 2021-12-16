using Web.API.Configuration;
using Web.API.Configuration.Middlewares;

class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();
        services.AddSwagger();
        services.AddAutoMapper();
        services.ConfigureApi();

        services.AddCatalogService(Configuration);
        services.AddBasketService(Configuration);
        services.AddOrderingService(Configuration);
        services.AddIdentityService(Configuration);

        services.AddHttpContextAccessor();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseMiddleware<CustomExceptionHandlerMiddleware>();

        app.UseSwagger();
        app.UseSwaggerUI(setup =>
        {
            setup.SwaggerEndpoint("/swagger/v1/swagger.json", "Web.API V1");
        });

        app.UseRouting();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}