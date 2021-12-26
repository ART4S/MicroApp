using Catalog.API.Models;
using MongoDB.Driver;

namespace Catalog.API.DataAccess.Repositories;

public class CatalogBrandRepository : ICatalogBrandRepository
{
    private readonly IMongoCollection<CatalogBrand> _collection;
    private readonly IClientSessionHandle _session;

    public CatalogBrandRepository(
        IMongoCollection<CatalogBrand> collection,
        IClientSessionHandle session)
    {
        _collection = collection;
        _session = session;
    }

    public async Task<ICollection<CatalogBrand>> GetAll()
    {
        return await _collection.Find(_session, _ => true).ToListAsync();
    }

    public async Task<CatalogBrand?> GetById(Guid brandId)
    {
        return await _collection.Find(_session, x => x.Id == brandId).FirstOrDefaultAsync();
    }

    public Task<bool> Exists(Guid brandId)
    {
        return _collection.Find(_session, x => x.Id == brandId).AnyAsync();
    }

    public Task Create(CatalogBrand brand)
    {
        return _collection.InsertOneAsync(_session, brand);
    }

    public Task CreateMany(IEnumerable<CatalogBrand> brands)
    {
        return _collection.InsertManyAsync(brands);
    }
}
