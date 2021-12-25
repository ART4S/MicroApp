using AutoMapper;
using Catalog.API.Application.Models.CatalogBrand;
using Catalog.API.DataAccess;
using MediatR;

namespace Catalog.API.Application.Requests.Catalog.GetBrands;

public class GetBrandsRequestHandler : IRequestHandler<GetBrandsRequest, IEnumerable<CatalogBrandDto>>
{
    private readonly ICatalogDbContext _catalogDb;
    private readonly IMapper _mapper;

    public GetBrandsRequestHandler(ICatalogDbContext catalogDb, IMapper mapper)
    {
        _catalogDb = catalogDb;
        _mapper = mapper;
    }

    public async Task<IEnumerable<CatalogBrandDto>> Handle(GetBrandsRequest request, CancellationToken cancellationToken)
    {
        return (await _catalogDb.CatalogBrands.GetAll())
            .Select(_mapper.Map<CatalogBrandDto>);
    }
}
