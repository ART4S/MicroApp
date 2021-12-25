using Autofac.Extensions.DependencyInjection;
using Ordering.API.Configuration;

CreateHostBuilder(args).Build()
    .MigrateOrderingDbContext()
    .MigrateIntegrationDbContext()
    .MigrateIdempotencyDbContext()
    .Run();

static IHostBuilder CreateHostBuilder(string[] args) =>
    Host.CreateDefaultBuilder(args)
        .UseServiceProviderFactory(new AutofacServiceProviderFactory())
        .ConfigureWebHostDefaults(webBuilder =>
        {
            webBuilder.UseStartup<Startup>();
        });
