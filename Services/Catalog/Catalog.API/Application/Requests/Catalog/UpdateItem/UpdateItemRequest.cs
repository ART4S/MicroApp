using Catalog.API.Application.Models.CatalogItem;
using Catalog.API.Application.Requests.Abstractions;

namespace Catalog.API.Application.Requests.Catalog.UpdateItem;

public record UpdateItemRequest(Guid ProductId, CatalogItemEditDto Product) : Command;