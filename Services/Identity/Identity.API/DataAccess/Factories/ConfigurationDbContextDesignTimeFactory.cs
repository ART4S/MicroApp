using IdentityServer4.EntityFramework.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Identity.API.DataAccess.Factories;

class ConfigurationDbContextDesignTimeFactory : IDesignTimeDbContextFactory<ConfigurationDbContext>
{
    public ConfigurationDbContext CreateDbContext(string[] args)
    {
        DbContextOptionsBuilder<ConfigurationDbContext> builder = new();

        builder.UseSqlServer("Server=.;Port=1433;Database=identity_db;User Id=sa;Password=Qwerty123", options =>
        {
            options.MigrationsAssembly(GetType().Assembly.FullName);
        });

        return new ConfigurationDbContext(builder.Options, new());
    }
}
