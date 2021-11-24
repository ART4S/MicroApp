using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Catalog.Infrastructure.DataAccess
{
    internal class CatalogDbContextDesignTimeFactory : IDesignTimeDbContextFactory<CatalogDbContext>
    {
        public CatalogDbContext CreateDbContext(string[] args)
        {
            DbContextOptionsBuilder<CatalogDbContext> builder = new();

            builder.UseSqlServer("Server=.;Port=1433;Database=catalog_db;User Id=sa;Password=Qwerty123", options =>
            {
                options.MigrationsAssembly(GetType().Assembly.FullName);
            });

            return new CatalogDbContext(builder.Options);
        }
    }
}
