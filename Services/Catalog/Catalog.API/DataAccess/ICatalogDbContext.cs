using Catalog.API.DataAccess.Repositories;

namespace Catalog.API.DataAccess;

public interface ICatalogDbContext
{
    IProductsRepository Products { get; }
    ICatalogBrandRepository CatalogBrands { get; }
    ICatalogTypeRepository CatalogTypes { get; }
    Task SaveChanges();
}
