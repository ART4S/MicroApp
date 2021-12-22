using HostConfiguration;
using Identity.API.DataAccess;
using Identity.API.Models.Entities;
using Identity.API.Settings;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Identity;

namespace Identity.API.Configuration;

static class HostConfiguration
{
    public static IHost MigrateAppDbContext(this IHost host)
    {
        host.CreateDbContext<AppDbContext>();

        return host;
    }

    public static IHost MigrateIdentityDbContexts(this IHost host)
    {
        var config = host.Services.GetRequiredService<IConfiguration>();
        ClientsSettings settings = new();
        config.GetSection("ClientsSettings").Bind(settings);

        host.MigrateDbContext<ConfigurationDbContext>((services, context) =>
        {
            context.Clients.AddRange(new[]
            {
                new Client
                {
                    ClientId = settings.Spa.ClientId,
                    ClientSecrets = { new(settings.Spa.ClientSecret.Sha256()) },
                    AllowedGrantTypes = GrantTypes.Code,
                    RequirePkce = true,
                    AlwaysIncludeUserClaimsInIdToken = true,
                    RedirectUris = settings.Spa.RedirectUris,
                    AllowedScopes = settings.Spa.AllowedScopes,
                }
            }.Select(x => x.ToEntity()));

            context.IdentityResources.AddRange(new[]
            {
                new IdentityResource(
                    name: "openid",
                    userClaims: new[] { "sub" })
            }.Select(x => x.ToEntity()));

            context.SaveChanges();
        });

        host.MigrateDbContext<PersistedGrantDbContext>();

        return host;
    }

    public static IHost SeedInitialData(this IHost host)
    {
        IServiceScope scope = host.Services.CreateScope();

        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

        // TODO: seed users from xml
        var res = userManager.CreateAsync(new()
        {
            Id = new Guid("7f6e4cf9-ac94-4a91-bbe5-e88dcf7a3980"),
            UserName = "Test"
        },"test").Result;

        return host;
    }
}