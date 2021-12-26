using Catalog.API.Models;
using MongoDB.Driver;

namespace Catalog.API.DataAccess.Repositories;

public class ProductRepository : IProductsRepository
{
    private readonly IMongoCollection<CatalogItem> _collection;
    private readonly IClientSessionHandle _session;

    public ProductRepository(
        IMongoCollection<CatalogItem> collection,
        IClientSessionHandle session)
    {
        _collection = collection;
        _session = session;
    }

    public async Task<ICollection<CatalogItem>> GetAll()
    {
        return await _collection.Find(_session, _ => true).ToListAsync();
    }

    public async Task<ICollection<CatalogItem>> GetByIds(Guid[] ids)
    {
        return await _collection.Find(_session, x => ids.Contains(x.Id)).ToListAsync();
    }

    public async Task<CatalogItem?> GetById(Guid productId)
    {
        return await _collection.Find(_session, x => x.Id == productId).SingleOrDefaultAsync();
    }

    public Task Create(CatalogItem product)
    {
        return _collection.InsertOneAsync(_session, product);
    }

    public Task CreateMany(IEnumerable<CatalogItem> products)
    {
        return _collection.InsertManyAsync(products);
    }

    public async Task Update(CatalogItem product)
    {
        await _collection.ReplaceOneAsync(_session, x => x.Id == product.Id, product);
    }

    public Task Remove(Guid productId)
    {
        return _collection.DeleteOneAsync(_session, x => x.Id == productId);
    }
}