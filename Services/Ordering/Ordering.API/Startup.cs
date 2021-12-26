using Autofac;
using Ordering.API.Configuration;

class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddAppServices();
        services.AddIdentityServices();
        services.AddControllers();
        services.AddSwagger();
        services.AddOrderingDbContext(Configuration);
        services.AddIntegrationServices(Configuration);
        services.AddIdempotencyServices(Configuration);
        services.AddBuyerService();
        services.AddMediator();
        services.AddAutoMapper();
        services.AddEventHandlers();
        services.AddValidation();
        services.ConfigureApi();
        services.AddTaskScheduling(Configuration);
        services.AddHealthChecks(Configuration);
        services.AddCustomAuthentication(Configuration);
        services.AddCustomAuthorization();
    }

    public void ConfigureContainer(ContainerBuilder builder)
    {
        builder.RegisterAssemblyModules(typeof(Startup));
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseCustomExceptionHandler();

        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(setup =>
            {
                setup.RoutePrefix = "";
                setup.SwaggerEndpoint("swagger/v1/swagger.json", "Ordering.API");
            });
        }

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints => 
        { 
            endpoints.MapControllers();
            endpoints.MapHealthChecks();
        });

        app.SubscribeToEvents();
    }
}
