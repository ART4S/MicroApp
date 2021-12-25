using Autofac;
using Payment.API.Configuration;

namespace Payment.API;

class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddIntegrationServices(Configuration);
        services.AddEventHandlers();
        services.AddHealthChecks(Configuration);
    }

    public void ConfigureContainer(ContainerBuilder builder)
    {
        builder.RegisterAssemblyModules(typeof(Startup));
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseRouting();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapHealthChecks();
        });

        app.SubscribeToEvents();
    }
}