using Catalog.Domian.Entities;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Infrastructure.DataAccess
{
    public class CatalogDbContext : DbContext
    {
        public CatalogDbContext(DbContextOptions<CatalogDbContext> options) : base(options)
        {
        }

        public DbSet<CatalogItem> CatalogItems { get; set; }
        public DbSet<CatalogItemType> CatalogItemTypes { get; set; }
        public DbSet<CatalogItemBrand> CatalogItemBrands { get; set; }
    }
}
