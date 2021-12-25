using Autofac.Extensions.DependencyInjection;
using Catalog.API.Configuration;

CreateHostBuilder(args).Build()
    .InitCatalogDb()
    .InitIntegrationDb()
    .Run();

static IHostBuilder CreateHostBuilder(string[] args) =>
    Host.CreateDefaultBuilder(args)
        .UseServiceProviderFactory(new AutofacServiceProviderFactory())
        .ConfigureWebHostDefaults(webBuilder =>
        {
            webBuilder.UseWebRoot("WebContent")
                .UseStartup<Startup>();
        });
