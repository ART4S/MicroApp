using Catalog.Application.Dto.CatalogItem;
using Catalog.Application.Requests.Abstractions;

namespace Catalog.Application.Requests.Catalog.UpdateItem;

public record UpdateItemRequest(Guid Id, CatalogItemEditDto Item) : Command;