using AutoMapper;
using Catalog.API.Application.Models.CatalogType;
using Catalog.API.DataAccess;
using MediatR;

namespace Catalog.API.Application.Requests.Catalog.GetTypes;

public class GetTypesRequestHandler : IRequestHandler<GetTypesRequest, IEnumerable<CatalogTypeDto>>
{
    private readonly ICatalogDbContext _catalogDb;
    private readonly IMapper _mapper;

    public GetTypesRequestHandler(ICatalogDbContext catalogDb, IMapper mapper)
    {
        _catalogDb = catalogDb;
        _mapper = mapper;
    }

    public async Task<IEnumerable<CatalogTypeDto>> Handle(GetTypesRequest request, CancellationToken cancellationToken)
    {
        return (await _catalogDb.CatalogTypes.GetAll())
            .Select(_mapper.Map<CatalogTypeDto>);
    }
}
