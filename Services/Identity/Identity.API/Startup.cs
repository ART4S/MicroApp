using Identity.API.Configuration;
using Identity.API.Settings;
using Microsoft.EntityFrameworkCore;

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

        string connectionString = Configuration.GetConnectionString("DefaultConnection");

        services.AddCustomIdentity(connectionString);
        services.AddCustomIdentityServer(connectionString);

        JwtSettings settings = new();

        Configuration.GetSection("JwtSettings").Bind(settings);

        services.AddSingleton(settings);
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(setup =>
            {
                setup.SwaggerEndpoint("/swagger/v1/swagger.json", "Identity.API V1");
            });
        }

        app.UseIdentityServer();

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}