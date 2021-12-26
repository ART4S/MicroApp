using Catalog.API.Models;

namespace Catalog.API.DataAccess.Repositories;

public interface ICatalogTypeRepository
{
    Task<ICollection<CatalogType>> GetAll();
    Task<CatalogType?> GetById(Guid typeId);
    Task<bool> Exists(Guid typeId);
    Task Create(CatalogType type);
    Task CreateMany(IEnumerable<CatalogType> types);
}
