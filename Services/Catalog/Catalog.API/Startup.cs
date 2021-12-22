using Autofac;
using Catalog.API.Configuration;

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
        services.AddCatalogDbContext(Configuration);
        services.AddIntegrationServices(Configuration);
        services.AddEventHandlers();
        services.AddTaskScheduling(Configuration);
        services.AddRepositories();
        services.AddMediator();
        services.AddAutoMapper();
        services.AddValidation();
        services.ConfigureApi();
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
            setup.SwaggerEndpoint("/swagger/swagger.json", "Catalog.API");
        });

        app.UseRouting();

        app.UseEndpoints(endpoints => 
        { 
            endpoints.MapControllers();
        });

        app.SubscribeToEvents();
    }
}
