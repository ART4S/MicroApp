using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    CreateHostBuilder(args).Build().Run();
}
catch (Exception ex)
{
    Log.Logger.Fatal(ex, "Unexpected error occured while initializing host");
}
finally
{
    Log.CloseAndFlush();
}

static IHostBuilder CreateHostBuilder(string[] args) =>
    Host.CreateDefaultBuilder(args)
        .UseSerilog((host, services, logConfig) => logConfig
            .ReadFrom.Configuration(host.Configuration)
            .Enrich.FromLogContext()
            .WriteTo.Console())
        .ConfigureWebHostDefaults(webBuilder =>
        {
            webBuilder.UseStartup<Startup>();
        });

