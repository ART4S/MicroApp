using Catalog.API.Infrastructure.Attributes;
using Catalog.Application.Interfaces.DataAccess;
using Catalog.Application.PipelineBehaviours;
using Catalog.Infrastructure.DataAccess.Catalog;
using Catalog.Infrastructure.DataAccess.Catalog.Repositories;
using EventBus.RabbitMQ.DependencyInjection;
using FluentValidation;
using FluentValidation.AspNetCore;
using IntegrationServices;
using IntegrationServices.DataAccess;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace Catalog.API.Configuration;

static class ServicesConfiguration
{
    public static void AddSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new()
            {
                Title = "MicroApp - Catalog.API",
                Version = "v1"
            });
        });
    }

    public static void AddCatalogDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        string connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddScoped<DbConnection>(_ => new SqlConnection(connectionString));

        services.AddDbContext<CatalogDbContext>((sp, options) =>
        {
            options.UseSqlServer(
                connection: sp.GetRequiredService<DbConnection>(),
                sqlOptions => sqlOptions.MigrationsAssembly(typeof(CatalogDbContext).Assembly.FullName));
        });

        services.AddScoped<ICatalogDbContext, CatalogDbContext>();
    }

    public static void AddIntegrationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddRabbitMQEventBus(configuration);

        services.AddDbContext<IntegrationDbContext>((sp, options) =>
        {
            options.UseSqlServer(
                connection: sp.GetRequiredService<DbConnection>(), 
                sqlOptions => sqlOptions.MigrationsAssembly(typeof(CatalogDbContext).Assembly.FullName));
        });

        services.AddScoped<IIntegrationDbContext, IntegrationDbContext>();

        services.AddScoped<IIntegrationEventService, IntegrationEventService>();
    }

    public static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IPictureRepository, PictureRepository>();
    }

    public static void AddMediator(this IServiceCollection services)
    {
        services.AddMediatR(typeof(ICatalogDbContext));

        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(SaveChangesBehaviour<,>));
    }

    public static void AddAutoMapper(this IServiceCollection services)
    {
        services.AddAutoMapper(config =>
        {
            config.AddMaps(typeof(ICatalogDbContext));
        });
    }

    public static void AddValidation(this IServiceCollection services)
    {
        services.AddMvc(opt =>
        {
            opt.Filters.Add<ValidationAttribute>();
        }).AddFluentValidation();

        services.AddValidatorsFromAssemblyContaining(typeof(ICatalogDbContext));
    }

    public static void ConfigureApi(this IServiceCollection services)
    {
        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.SuppressModelStateInvalidFilter = true;
        });
    }
}