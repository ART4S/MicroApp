using IntegrationServices.Models;
using System.Linq.Expressions;

namespace IntegrationServices.Mongo;

public interface IMongoIntegrationDbContext
{
    Task<IntegrationEventLogEntry?> GetById(Guid eventId);

    Task<ICollection<IntegrationEventLogEntry>> GetAll(
        Expression<Func<IntegrationEventLogEntry, bool>> filter,
        Expression<Func<IntegrationEventLogEntry, object>> orderBy);

    Task Add(IntegrationEventLogEntry eventEntry);

    Task Update(IntegrationEventLogEntry eventEntry);

    Task SaveChanges();
}
