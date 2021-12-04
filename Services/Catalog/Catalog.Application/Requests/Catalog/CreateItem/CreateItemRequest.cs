using Catalog.Application.Abstractions;
using Catalog.Application.Dto.CatalogItem;

namespace Catalog.Application.Requests.Catalog.CreateItem;

public record CreateItemRequest(CatalogItemEditDto Item) :  Command<Guid>;