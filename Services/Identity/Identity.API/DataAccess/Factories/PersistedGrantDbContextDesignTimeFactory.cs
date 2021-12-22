using IdentityServer4.EntityFramework.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Identity.API.DataAccess.Factories;

class PersistedGrantDbContextDesignTimeFactory : IDesignTimeDbContextFactory<PersistedGrantDbContext>
{
    public PersistedGrantDbContext CreateDbContext(string[] args)
    {
        DbContextOptionsBuilder<PersistedGrantDbContext> builder = new();

        builder.UseSqlServer("Server=.;Port=1433;Database=identity_db;User Id=sa;Password=Qwerty123", options =>
        {
            options.MigrationsAssembly(GetType().Assembly.FullName);
        });

        return new PersistedGrantDbContext(builder.Options, new());
    }
}
