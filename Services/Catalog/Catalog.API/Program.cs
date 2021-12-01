using Autofac.Extensions.DependencyInjection;
using Catalog.API.Configuration;

IHost host = CreateHostBuilder(args).Build()
    .MigrateCatalogDbContext()
    .MigrateIntegrationDbContext();

host.Run();

static IHostBuilder CreateHostBuilder(string[] args) =>
    Host.CreateDefaultBuilder(args)
        .UseServiceProviderFactory(new AutofacServiceProviderFactory())
        .ConfigureWebHostDefaults(webBuilder =>
        {
            webBuilder.UseWebRoot("WebContent")
                .UseStartup<Startup>();
        });
