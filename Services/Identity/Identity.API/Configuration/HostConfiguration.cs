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
        var config = host.Services.GetRequiredService<IConfiguration>();
        var clearDb = config.GetValue<bool>("ClearDatabase");

        if (clearDb)
        {
            host.CreateDbContext<AppDbContext>((services, context) =>
            {
                var userManager = services.GetRequiredService<UserManager<User>>();

                // TODO: seed from xml
                var res = userManager.CreateAsync(new()
                {
                    Id = new Guid("7f6e4cf9-ac94-4a91-bbe5-e88dcf7a3980"),
                    UserName = "Test"
                }, "test").Result;
            });
        }
        else
            host.MigrateDbContext<AppDbContext>();

        return host;
    }

    public static IHost MigrateIdentityDbContexts(this IHost host)
    {
        var config = host.Services.GetRequiredService<IConfiguration>();
        var clearDb = config.GetValue<bool>("ClearDatabase");

        if (clearDb)
        {
            ClientsSettings clientSettings = new();
            config.GetSection("ClientsSettings").Bind(clientSettings);

            host.MigrateDbContext<ConfigurationDbContext>((services, context) =>
            {
                context.Clients.AddRange(new[]
                {
                    new Client
                    {
                        ClientId = clientSettings.Spa.ClientId,
                        ClientSecrets = { new(clientSettings.Spa.ClientSecret.Sha256()) },
                        AllowedGrantTypes = GrantTypes.Code,
                        RequirePkce = true,
                        AlwaysIncludeUserClaimsInIdToken = true,
                        RedirectUris = clientSettings.Spa.RedirectUris,
                        AllowedScopes = clientSettings.Spa.AllowedScopes,
                        IdentityTokenLifetime = clientSettings.Spa.IdTokenLifetimeSec,
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
        }
        else
            host.MigrateDbContext<ConfigurationDbContext>();

        host.MigrateDbContext<PersistedGrantDbContext>();

        return host;
    }
}