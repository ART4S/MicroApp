using Catalog.API.Application.Models.CatalogItem;
using Catalog.API.Application.Requests.Abstractions;

namespace Catalog.API.Application.Requests.Catalog.CreateItem;

public record CreateItemRequest(CatalogItemEditDto Product) : Command<Guid>;