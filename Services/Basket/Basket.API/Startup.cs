using Basket.API.Configuration;
using GrpcBasket;

class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddGrpc();
        services.AddGrpcReflection();
        services.AddDataAccess(Configuration);
        services.AddIntegrationServices(Configuration);
        services.AddAutoMapper();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseCustomExceptionHandler();

        app.UseRouting();

        app.UseEndpoints(endpoints =>
        {
            if (env.IsDevelopment())
            {
                endpoints.MapGrpcReflectionService();
            }

            endpoints.MapGrpcService<BasketService>();
        });

        app.SubscribeToEvents();
    }
}
