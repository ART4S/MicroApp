using AutoMapper;
using Catalog.API.Application.Models.CatalogItem;
using Catalog.API.DataAccess;
using MediatR;

namespace Catalog.API.Application.Requests.Catalog.GetItems;

public class GetItemsRequestHandler : IRequestHandler<GetItemsRequest, IEnumerable<CatalogItemDto>>
{
    private readonly ICatalogDbContext _catalogDb;
    private readonly IMapper _mapper;

    public GetItemsRequestHandler(ICatalogDbContext catalogDb, IMapper mapper)
    {
        _catalogDb = catalogDb;
        _mapper = mapper;
    }

    public async Task<IEnumerable<CatalogItemDto>> Handle(GetItemsRequest request, CancellationToken cancellationToken)
    {
        return (await _catalogDb.Products.GetAll())
            .Select(_mapper.Map<CatalogItemDto>);
    }
}
