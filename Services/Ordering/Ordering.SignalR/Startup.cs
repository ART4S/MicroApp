using Ordering.SignalR;
using Ordering.SignalR.Configuration;

class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddSignalR();

        services.AddCustomAuthentication(Configuration);
        services.AddCustomAuthorization();

        services.AddRabbitMq(Configuration);
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseFileServer();

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints => 
        {
            endpoints.MapHub<NotificationsHub>("/notifications");
        });

        app.SubscribeToEvents();
    }
}
