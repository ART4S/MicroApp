using Catalog.Application.Dto.CatalogItem;
using Catalog.Application.Requests.Abstractions;

namespace Catalog.Application.Requests.Catalog.CreateItem;

public record CreateItemRequest(CatalogItemEditDto Item) :  Command<Guid>;