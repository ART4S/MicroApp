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
        services.AddMediator();
        services.AddAutoMapper();
        services.AddEventHandlers();
        services.AddValidation();
        services.ConfigureApi();
        services.AddTaskScheduling(Configuration);
    }

    public void ConfigureContainer(ContainerBuilder builder)
    {
        builder.RegisterAssemblyModules(typeof(Startup));
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseCustomExceptionHandler();

        app.UseSwagger();
        app.UseSwaggerUI(setup =>
        {
            setup.SwaggerEndpoint("/swagger/v1/swagger.json", "Ordering.API V1");
        });

        app.UseRouting();

        app.UseEndpoints(endpoints => 
        { 
            endpoints.MapControllers();
        });

        app.SubscribeToEvents();
    }
}
