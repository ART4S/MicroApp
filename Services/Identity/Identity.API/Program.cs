using Identity.API.Configuration;

CreateHostBuilder(args).Build()
    .MigrateAppDbContext()
    .MigrateIdentityDbContexts()
    .SeedInitialData()
    .Run();

static IHostBuilder CreateHostBuilder(string[] args) =>
    Host.CreateDefaultBuilder(args)
        .ConfigureWebHostDefaults(webBuilder =>
        {
            webBuilder.UseStartup<Startup>();
        });
