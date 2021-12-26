using Catalog.API.DataAccess.Repositories;
using Catalog.API.Models;
using MongoDB.Driver;

namespace Catalog.API.DataAccess;

public class CatalogDbContext : ICatalogDbContext, IDisposable
{
    private readonly IMongoDatabase _db;
    private readonly IClientSessionHandle _session;

    public CatalogDbContext(IMongoDatabase db)
    {
        _db = db;
        _session = db.Client.StartSession();
        //_session.StartTransaction();
    }

    private ProductRepository? _products;
    public IProductsRepository Products => _products ??= new ProductRepository(_db.GetCollection<CatalogItem>("Products"), _session);

    private CatalogBrandRepository? _brands;
    public ICatalogBrandRepository CatalogBrands => _brands ??= new CatalogBrandRepository(_db.GetCollection<CatalogBrand>("CatalogBrands"), _session);

    private CatalogTypeRepository? _types;
    public ICatalogTypeRepository CatalogTypes => _types ??= new CatalogTypeRepository(_db.GetCollection<CatalogType>("CatalogTypes"), _session);

    public async Task SaveChanges()
    {
        //if (_session.IsInTransaction)
        //{
        //    try
        //    {
        //        await _session.CommitTransactionAsync();
        //    }
        //    catch
        //    {
        //        await _session.AbortTransactionAsync();
        //        throw;
        //    }
        //}

        //_session.StartTransaction();
    }

    public void Dispose()
    {
        //_session.AbortTransaction();
        _session.Dispose();
    }
}
