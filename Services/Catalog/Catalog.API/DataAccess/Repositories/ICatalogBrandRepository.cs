using Catalog.API.Models;

namespace Catalog.API.DataAccess.Repositories;

public interface ICatalogBrandRepository
{
    Task<ICollection<CatalogBrand>> GetAll();
    Task<CatalogBrand?> GetById(Guid brandId); 
    Task<bool> Exists(Guid brandId);
    Task Create(CatalogBrand brand);
}
