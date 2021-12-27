using Basket.API.Configuration;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Serilog;
using System.Net;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    CreateHostBuilder(args).Build().InitDatabase().Run();
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
            IConfiguration envConfig = GetEnvironmentConfig();

            webBuilder
                .ConfigureKestrel(options =>
                {
                    options.Listen(IPAddress.Any, envConfig.GetValue<int>("HttpPort"), listenOptions =>
                    {
                        listenOptions.Protocols = HttpProtocols.Http1AndHttp2;
                    });

                    options.Listen(IPAddress.Any, envConfig.GetValue<int>("GrpcPort"), listenOptions =>
                    {
                        listenOptions.Protocols = HttpProtocols.Http2;
                    });
                })
                .UseStartup<Startup>();
        });

static IConfiguration GetEnvironmentConfig() =>
    new ConfigurationBuilder()
        .AddEnvironmentVariables()
        .Build();