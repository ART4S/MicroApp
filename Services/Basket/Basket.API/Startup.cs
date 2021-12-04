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

        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new()
            {
                Title = "MicroApp - Basket.API",
                Version = "v1"
            });
        });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        //app.UseCustomExceptionHandler();

        app.UseSwagger();
        app.UseSwaggerUI(setup =>
        {
            setup.SwaggerEndpoint("/swagger/v1/swagger.json", "Basket.API V1");
        });

        app.UseRouting();

        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });

        //app.SubscribeToEvents();
    }
}
