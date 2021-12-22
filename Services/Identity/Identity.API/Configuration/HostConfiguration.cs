using HostConfiguration;
using Identity.API.DataAccess;
using Identity.API.Models.Entities;
using IdentityServer4;
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
        host.MigrateDbContext<ConfigurationDbContext>((services, context) =>
        {

            context.Clients.AddRange(new[]
            {
                new Client
                {
                    ClientId = "client",

                    AllowedGrantTypes = GrantTypes.ClientCredentials,

                    // secret for authentication
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },

                    AllowAccessTokensViaBrowser = true,

                    RedirectUris = { "http://localhost:4000/api/v1/auth/test" },

                    AllowedScopes = { "openid" },
                }
            }.Select(x => x.ToEntity())); ;

            context.ApiScopes.AddRange(new[]
            {
                new ApiScope("scope")
            }.Select(x => x.ToEntity()));


            context.IdentityResources.AddRange(new[]
            {
                new IdentityResource(
                    name: "openid",
                    userClaims: new[] { "sub" },
                    displayName: "Your user identifier"),

                new IdentityResource(
                    name: "profile",
                    userClaims: new[] { "sub", "name", "email", "website" },
                    displayName: "Your profile data")
            }.Select(x => x.ToEntity()));

            context.ApiResources.AddRange(new List<ApiResource>
            {
                new ApiResource("api1", "My API"){ Scopes = { "scope"}}
            }.Select(x => x.ToEntity()));

            context.SaveChanges();
        });

        host.MigrateDbContext<PersistedGrantDbContext>();

        return host;
    }

    public static IHost AddDefaultUsers(this IHost host)
    {
        IServiceScope scope = host.Services.CreateScope();

        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

        var res = userManager.CreateAsync(new()
        {
            Id = new Guid("7f6e4cf9-ac94-4a91-bbe5-e88dcf7a3980"),
            UserName = "Test"
        },"test").Result;

        return host;
    }
}

//public class Config
//{
//    public static IEnumerable<Client> Clients = new List<Client>
//        {
//            new Client
//            {
//                ClientId = "spa",
//                AllowedGrantTypes = GrantTypes.Code,
//                RequireClientSecret = false,
//                RequirePkce = true,
//                RequireConsent = false,
//                RedirectUris = {
//                    "https://localhost:4000/callback.html",
//                    "https://localhost:4000/popup.html",
//                    "https://localhost:4000/silent.html"
//                },
//                PostLogoutRedirectUris = { "http://localhost:4000/index.html" },
//                AllowedScopes = { "openid", "profile", "email", IdentityServerConstants.LocalApi.ScopeName },
//            },
//        };

//    public static IEnumerable<IdentityResource> IdentityResources = new List<IdentityResource>
//        {
//            new IdentityResources.OpenId(),
//            new IdentityResources.Profile(),
//        };

//    public static IEnumerable<ApiResource> Apis = new List<ApiResource>
//    {
//        // local API
//        new ApiResource(IdentityServerConstants.LocalApi.ScopeName),
//    };
//}

//new Client()
//{
//    ClientId = "spa",
//    AllowedGrantTypes = GrantTypes.ClientCredentials,
//    RequireClientSecret = false,
//    RequirePkce = false,
//    RequireConsent = false,
//    AlwaysSendClientClaims = true,
//    AlwaysIncludeUserClaimsInIdToken = true,
//    AllowedScopes =
//        {
//            IdentityServerConstants.StandardScopes.OpenId,
//            IdentityServerConstants.StandardScopes.Profile,
//            IdentityServerConstants.LocalApi.ScopeName,
//            "web",
//        },
//}.ToEntity()


//context.Clients.AddRange(new[]
//            {
//                new Client()
//{
//    ClientId = "postman",
//                    AllowedGrantTypes = GrantTypes.Code,
//                    RequirePkce = false,
//                    RequireConsent = false,
//                    AlwaysSendClientClaims = true,
//                    AlwaysIncludeUserClaimsInIdToken = true,
//                    AllowAccessTokensViaBrowser = true,
//                    RequireClientSecret = false,
//                    AllowedScopes =
//                        {
//        "openid",
//                            "profile",
//                            "web",
//                            IdentityServerConstants.LocalApi.ScopeName,
//                        }
//},

//                new Client()
//{
//    ClientId = "web",
//                    AllowedGrantTypes = GrantTypes.Implicit,
//                    RequireClientSecret = false,
//                    RequirePkce = false,
//                    RequireConsent = false,
//                    AlwaysSendClientClaims = true,
//                    AlwaysIncludeUserClaimsInIdToken = true,
//                    AllowAccessTokensViaBrowser = true,
//                    AllowedScopes =
//                    {
//        "openid",
//                        "profile",
//                        "web",
//                        IdentityServerConstants.LocalApi.ScopeName,
//                    },
//                },
//            }.Select(x => x.ToEntity()));

//context.ApiScopes.AddRange(new[]
//{
//                new ApiScope("web"),
//                new ApiScope(IdentityServerConstants.LocalApi.ScopeName)
//            }.Select(x => x.ToEntity()));


//context.IdentityResources.AddRange(new[]
//{
//              new IdentityResource(
//                name: "openid",
//                userClaims: new[] { "sub" },
//                displayName: "Your user identifier"),

//                new IdentityResource(
//                    name: "profile",
//                    userClaims: new[] { "sub", "name", "email", "website" },
//                    displayName: "Your profile data")
//            }.Select(x => x.ToEntity()));

//context.ApiResources.AddRange(new List<ApiResource>
//{
//    //new ApiResource("web", "Web.API"){ Scopes = { "web" }},

//}.Select(x => x.ToEntity()));