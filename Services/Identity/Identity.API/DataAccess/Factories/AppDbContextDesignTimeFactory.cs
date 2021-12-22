using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Identity.API.DataAccess.Factories;

class AppDbContextDesignTimeFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        DbContextOptionsBuilder<AppDbContext> builder = new();

        builder.UseSqlServer("Server=.;Port=1433;Database=identity_db;User Id=sa;Password=Qwerty123", options =>
        {
            options.MigrationsAssembly(GetType().Assembly.FullName);
        });

        return new AppDbContext(builder.Options);
    }
}
