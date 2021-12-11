using Catalog.Application.Requests.Abstractions;

namespace Catalog.Application.Requests.Catalog.DeleteItem;

public record DeleteItemRequest(Guid ItemId) : Command;