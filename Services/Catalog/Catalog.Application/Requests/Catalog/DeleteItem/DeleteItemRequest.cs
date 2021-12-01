using MediatR;

namespace Catalog.Application.Requests.Catalog.DeleteItem;

public record DeleteItemRequest(Guid Id) : IRequest;