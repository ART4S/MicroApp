using Catalog.Domian.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Catalog.Application.Interfaces.DataAccess;

public interface ICatalogDbContext
{
    DatabaseFacade Database { get; }
    Task<int> SaveChanges(CancellationToken cancellationToken = default);

    DbSet<CatalogItem> CatalogItems { get; }
    DbSet<CatalogBrand> CatalogBrands { get; }
    DbSet<CatalogType> CatalogTypes { get; }
}
