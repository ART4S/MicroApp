using Catalog.Application.Dto.CatalogBrand;
using MediatR;

namespace Catalog.Application.Requests.Catalog.GetBrands;

public record GetBrandsRequest : IRequest<IQueryable<CatalogBrandDto>>;
