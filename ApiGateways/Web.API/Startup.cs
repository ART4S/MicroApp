using Web.API.Configuration;
using Web.API.Configuration.Factories;
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
        HttpPolicyFactory.InitSettings(Configuration);

        services.AddControllers();
        services.AddHttpContextAccessor();
        services.AddSwagger();
        services.AddAutoMapper();
        services.ConfigureApi();

        services.AddCatalogService(Configuration);
        services.AddBasketService(Configuration);
        services.AddOrderingService(Configuration);
        services.AddUserService(Configuration);

        services.AddCustomAuthentication(Configuration);
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseMiddleware<CustomExceptionHandlerMiddleware>();

        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(setup =>
            {
                setup.RoutePrefix = "";
                setup.SwaggerEndpoint("swagger/v1/swagger.json", "Web.API");
            });
        }

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}