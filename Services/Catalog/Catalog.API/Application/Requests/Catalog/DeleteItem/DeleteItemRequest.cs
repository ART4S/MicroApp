using Catalog.API.Application.Requests.Abstractions;

namespace Catalog.API.Application.Requests.Catalog.DeleteItem;

public record DeleteItemRequest(Guid ProductId) : Command;