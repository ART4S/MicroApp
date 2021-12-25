using Catalog.API.Application.Models.CatalogItem;
using MediatR;

namespace Catalog.API.Application.Requests.Catalog.GetItems;

public record GetItemsRequest : IRequest<IEnumerable<CatalogItemDto>>;
