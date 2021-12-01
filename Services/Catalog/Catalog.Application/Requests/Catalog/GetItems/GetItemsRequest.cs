using Catalog.Application.Dto.CatalogItem;
using MediatR;

namespace Catalog.Application.Requests.Catalog.GetItems;

public record GetItemsRequest : IRequest<IQueryable<CatalogItemDto>>;
