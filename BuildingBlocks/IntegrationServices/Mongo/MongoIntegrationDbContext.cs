using IntegrationServices.Models;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace IntegrationServices.Mongo;

public class MongoIntegrationDbContext : IMongoIntegrationDbContext
{
    private readonly IMongoDatabase _db;
    private readonly IMongoCollection<IntegrationEventLogEntry> _collection;

    public MongoIntegrationDbContext(IMongoDatabase db)
    {
        _db = db;
        _collection = db.GetCollection<IntegrationEventLogEntry>("IntegrationEvents");
    }

    public Task<IntegrationEventLogEntry?> GetById(Guid eventId)
    {
        return _collection.Find(x => x.Id == eventId).FirstOrDefaultAsync();
    }

    public async Task<ICollection<IntegrationEventLogEntry>> GetAll(
        Expression<Func<IntegrationEventLogEntry, bool>> filter, 
        Expression<Func<IntegrationEventLogEntry, object>> orderBy)
    {
        return await _collection.Find(filter).SortBy(orderBy).ToListAsync();
    }

    public Task Add(IntegrationEventLogEntry eventEntry)
    {
        return _collection.InsertOneAsync(eventEntry);
    }

    public async Task Update(IntegrationEventLogEntry eventEntry)
    {
        await _collection.ReplaceOneAsync(x => x.Id == eventEntry.Id, eventEntry);
    }

    public async Task SaveChanges()
    {

    }
}
