using Catalog.Domian.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Catalog.Application.Services.DataAccess;

public interface ICatalogDbContext
{
    DatabaseFacade Database { get; }

    DbSet<CatalogItem> CatalogItems { get; }
    DbSet<CatalogBrand> CatalogBrands { get; }
    DbSet<CatalogTypeDict> CatalogTypes { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
