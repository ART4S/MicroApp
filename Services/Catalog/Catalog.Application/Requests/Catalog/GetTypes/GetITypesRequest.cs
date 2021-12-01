using Catalog.Application.Dto.CatalogType;
using MediatR;

namespace Catalog.Application.Requests.Catalog.GetTypes;

public record GetTypesRequest : IRequest<IQueryable<CatalogTypeDto>>;
