using Catalog.API.Models;
using MongoDB.Driver;

namespace Catalog.API.DataAccess.Repositories;

public class CatalogTypeRepository : ICatalogTypeRepository
{
    private readonly IMongoCollection<CatalogType> _collection;
    private readonly IClientSessionHandle _session;

    public CatalogTypeRepository(
        IMongoCollection<CatalogType> collection,
        IClientSessionHandle session)
    {
        _collection = collection;
        _session = session;
    }

    public async Task<ICollection<CatalogType>> GetAll()
    {
        return await _collection.Find(_session, _ => true).ToListAsync();
    }

    public async Task<CatalogType?> GetById(Guid typeId)
    {
        return await _collection.Find(_session, x => x.Id == typeId).FirstOrDefaultAsync();
    }

    public Task<bool> Exists(Guid typeId)
    {
        return _collection.Find(_session, x => x.Id == typeId).AnyAsync();
    }

    public Task Create(CatalogType type)
    {
        return _collection.InsertOneAsync(type);
    }
}
