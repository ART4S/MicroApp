using Autofac.Extensions.DependencyInjection;
using Ordering.API.Configuration;

IHost host = CreateHostBuilder(args).Build()
    .MigrateOrderingDbContext()
    .MigrateIntegrationDbContext()
    .MigrateIdempotencyDbContext();

host.Run();

static IHostBuilder CreateHostBuilder(string[] args) =>
    Host.CreateDefaultBuilder(args)
        .UseServiceProviderFactory(new AutofacServiceProviderFactory())
        .ConfigureWebHostDefaults(webBuilder =>
        {
            webBuilder.UseStartup<Startup>();
        });
