using Catalog.API.Application.Models.CatalogType;
using MediatR;

namespace Catalog.API.Application.Requests.Catalog.GetTypes;

public record GetTypesRequest : IRequest<IEnumerable<CatalogTypeDto>>;
