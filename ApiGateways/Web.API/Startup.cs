﻿using Web.API.Configuration;
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
        services.AddSwagger();
        services.AddAutoMapper();
        services.ConfigureApi();

        services.AddCatalogService(Configuration);
        services.AddBasketService(Configuration);
        services.AddOrderingService(Configuration);
        services.AddIdentityService(Configuration);

        services.AddCustomAuthentication(Configuration);

        services.AddHttpContextAccessor();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseMiddleware<CustomExceptionHandlerMiddleware>();

        app.UseSwagger();
        app.UseSwaggerUI(setup =>
        {
            setup.SwaggerEndpoint("/swagger/swagger.json", "Web.API");
        });

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}