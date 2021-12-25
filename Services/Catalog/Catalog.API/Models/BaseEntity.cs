using MongoDB.Bson.Serialization.Attributes;

namespace Catalog.API.Models;

public abstract class BaseEntity
{
    [BsonId]
    public Guid Id { get; set; }
}
