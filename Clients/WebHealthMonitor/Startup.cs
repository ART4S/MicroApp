using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

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
        services.AddHealthChecksUI().AddInMemoryStorage();
        services.AddHealthChecks().AddCheck(
            name: "self", 
            check: () => HealthCheckResult.Healthy());
    }

    public void Configure(IApplicationBuilder app)
    {
        app.UseRouting();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();

            endpoints.MapHealthChecksUI(options =>
            {
                options.UIPath = "/hc-ui";
            });

            endpoints.MapHealthChecks("/hc", new()
            {
                Predicate = x => x.Name == "self",
                AllowCachingResponses = false,
            });
        });
    }
}