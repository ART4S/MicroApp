using Catalog.API.Application.Models.CatalogBrand;
using MediatR;

namespace Catalog.API.Application.Requests.Catalog.GetBrands;

public record GetBrandsRequest : IRequest<IEnumerable<CatalogBrandDto>>;
