using AutoMapper;
using AutoMapper.QueryableExtensions;
using Catalog.Application.Dto.CatalogType;
using Catalog.Application.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Application.Requests.Catalog.GetTypes;

public class GetTypesRequestHandler : IRequestHandler<GetTypesRequest, IQueryable<CatalogTypeDto>>
{
    private readonly ICatalogDbContext _catalogDb;
    private readonly IMapper _mapper;

    public GetTypesRequestHandler(ICatalogDbContext catalogDb, IMapper mapper)
    {
        _catalogDb = catalogDb;
        _mapper = mapper;
    }

    public Task<IQueryable<CatalogTypeDto>> Handle(GetTypesRequest request, CancellationToken cancellationToken)
    {
        var types = _catalogDb.CatalogTypes
            .AsNoTracking()
            .ProjectTo<CatalogTypeDto>(_mapper.ConfigurationProvider);

        return Task.FromResult(types);
    }
}
