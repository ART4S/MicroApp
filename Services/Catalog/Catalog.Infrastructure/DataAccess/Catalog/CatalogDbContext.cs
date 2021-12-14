using Catalog.Application.Services.DataAccess;
using Catalog.Domian.Entities;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Infrastructure.DataAccess.Catalog;

public class CatalogDbContext : DbContext, ICatalogDbContext
{
    public CatalogDbContext(DbContextOptions<CatalogDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            GetType().Assembly, 
            (type) => type.Namespace.EndsWith("Catalog.EntityConfigurations"));

        base.OnModelCreating(modelBuilder);
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return base.SaveChangesAsync(cancellationToken);
    }

    public DbSet<CatalogItem> CatalogItems { get; set; }
    public DbSet<CatalogType> CatalogTypes { get; set; }
    public DbSet<CatalogBrand> CatalogBrands { get; set; }
}
