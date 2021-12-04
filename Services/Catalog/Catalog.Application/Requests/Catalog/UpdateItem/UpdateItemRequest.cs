using Catalog.Application.Abstractions;
using Catalog.Application.Dto.CatalogItem;

namespace Catalog.Application.Requests.Catalog.UpdateItem;

public record UpdateItemRequest(Guid Id, CatalogItemEditDto Item) : Command;