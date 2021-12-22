using Identity.API.DataAccess;
using Identity.API.Models.Entities;
using Identity.API.Services;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Identity.API.Configuration;

static class ServicesConfiguration
{
    private static Assembly CurrentAssembly => typeof(Startup).Assembly;

    public static void AddSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new()
            {
                Title = "MicroShop - Identity.API",
                Version = "v1"
            });
        });
    }

    public static void AddCustomIdentity(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlServer(connectionString, sqlOptions =>
            {
                sqlOptions.MigrationsAssembly(CurrentAssembly.FullName);
            });
        });

        services.AddIdentity<User, Role>(setup =>
        {
            setup.Password.RequireDigit = false;
            setup.Password.RequireLowercase = false;
            setup.Password.RequireUppercase = false;
            setup.Password.RequireNonAlphanumeric = false;
            setup.Password.RequiredLength = 1;
        })
        .AddEntityFrameworkStores<AppDbContext>()
        .AddDefaultTokenProviders();
    }

    public static void AddCustomIdentityServer(this IServiceCollection services, string connectionString)
    {
        services.AddIdentityServer(x =>
        {
            x.IssuerUri = "http://identity-api";
            x.Authentication.CookieLifetime = TimeSpan.FromHours(2);
            x.UserInteraction.ErrorUrl = "/api/v1/home/error";
        })
        .AddDeveloperSigningCredential()
        .AddAspNetIdentity<User>()
        //.AddInMemoryIdentityResources(new IdentityResource[]
        //{
        //      new IdentityResource(
        //        name: "openid",
        //        userClaims: new[] { "sub" },
        //        displayName: "Your user identifier"),

        //    new IdentityResource(
        //        name: "profile",
        //        userClaims: new[] { "sub", "name", "email", "website" },
        //        displayName: "Your profile data")
        //})
        //.AddInMemoryApiScopes(new ApiScope[] 
        //{
        //    new ApiScope("web"),
        //    new ApiScope(IdentityServerConstants.LocalApi.ScopeName)
        //})
        //.AddInMemoryClients(new [] 
        //{
        //    new Client()
        //    {
        //        ClientId = "postman",
        //        AllowedGrantTypes = GrantTypes.Code,
        //        RequirePkce = false,
        //        RequireConsent = false,
        //        AlwaysSendClientClaims = true,
        //        AlwaysIncludeUserClaimsInIdToken = true,
        //        AllowAccessTokensViaBrowser = true,
        //        RequireClientSecret = false,
        //        AllowedScopes =
        //        {
        //            "openid",
        //            "profile",
        //            "web",
        //            IdentityServerConstants.LocalApi.ScopeName,
        //        }
        //    },

        //    new Client()
        //    {
        //        ClientId = "web",
        //        AllowedGrantTypes = GrantTypes.Implicit,
        //        RequireClientSecret = false,
        //        RequirePkce = false,
        //        RequireConsent = false,
        //        AlwaysSendClientClaims = true,
        //        AlwaysIncludeUserClaimsInIdToken = true,
        //        AllowAccessTokensViaBrowser = true,
        //        AllowedScopes =
        //        {
        //            "openid",
        //            "profile",
        //            "web",
        //            IdentityServerConstants.LocalApi.ScopeName,
        //        },
        //    },
        //});
        .AddConfigurationStore(options =>
        {
            options.ConfigureDbContext = builder => builder.UseSqlServer(
                connectionString,
                sqlOptions =>
                {
                    sqlOptions.MigrationsAssembly(CurrentAssembly.FullName);
                });
        })
        .AddOperationalStore(options =>
        {
            options.ConfigureDbContext = builder => builder.UseSqlServer(
                connectionString,
                sqlOptions =>
                {
                    sqlOptions.MigrationsAssembly(CurrentAssembly.FullName);
                });
        });
        services.AddScoped<IProfileService, ProfileService>();
        services.AddScoped<IAuthService, AuthService>();

        services.AddLocalApiAuthentication();
    }
}

//context.Clients.Add(
//    new Client()
//{
//    ClientId = "spa",
//        AllowedGrantTypes = GrantTypes.ClientCredentials,
//        RequireClientSecret = false,
//        RequirePkce = false,
//        RequireConsent = false,
//        AlwaysSendClientClaims = true,
//        AlwaysIncludeUserClaimsInIdToken = true,
//        AllowedScopes =
//        {
//        IdentityServerConstants.StandardScopes.OpenId,
//                        IdentityServerConstants.StandardScopes.Profile,
//                        IdentityServerConstants.LocalApi.ScopeName,
//                        "web",
//        },
//    }.ToEntity());

//context.ApiScopes.Add(new ApiScope()
//{
//    Name = "web"
//}.ToEntity());

//context.IdentityResources.AddRange(
//    new IdentityResources.OpenId().ToEntity(),
//    new IdentityResources.Profile().ToEntity());

//context.ApiResources.AddRange(new List<ApiResource>
//            {
//                new ApiResource("web", "Web.API") { Scopes = { "web" } },
//            }.Select(x => x.ToEntity()));