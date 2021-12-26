using Catalog.API.Models;

namespace Catalog.API.DataAccess.Repositories;

public interface IProductsRepository
{
    Task<ICollection<CatalogItem>> GetAll();
    Task<ICollection<CatalogItem>> GetByIds(Guid[] productIds);
    Task<CatalogItem?> GetById(Guid productId);
    Task Create(CatalogItem product);
    Task CreateMany(IEnumerable<CatalogItem> products);
    Task Update(CatalogItem product);
    Task Remove(Guid productId);
}
