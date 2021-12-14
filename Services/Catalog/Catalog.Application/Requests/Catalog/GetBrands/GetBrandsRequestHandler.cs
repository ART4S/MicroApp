using AutoMapper;
using AutoMapper.QueryableExtensions;
using Catalog.Application.Dto.CatalogBrand;
using Catalog.Application.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Application.Requests.Catalog.GetBrands;

public class GetBrandsRequestHandler : IRequestHandler<GetBrandsRequest, IQueryable<CatalogBrandDto>>
{
    private readonly ICatalogDbContext _catalogDb;
    private readonly IMapper _mapper;

    public GetBrandsRequestHandler(ICatalogDbContext catalogDb, IMapper mapper)
    {
        _catalogDb = catalogDb;
        _mapper = mapper;
    }

    public Task<IQueryable<CatalogBrandDto>> Handle(GetBrandsRequest request, CancellationToken cancellationToken)
    {
        var brands = _catalogDb.CatalogBrands
            .AsNoTracking()
            .ProjectTo<CatalogBrandDto>(_mapper.ConfigurationProvider);

        return Task.FromResult(brands);
    }
}
