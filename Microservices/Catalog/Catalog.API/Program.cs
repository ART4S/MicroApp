using Catalog.API.Configuration;
using Microsoft.AspNetCore;

IWebHost host = CreateWebHostBuilder(args).Build().MigrateCatalogDbContext();

host.Run();

static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
    WebHost.CreateDefaultBuilder(args)
        .UseWebRoot("WebContent")
        .UseStartup<Startup>();
