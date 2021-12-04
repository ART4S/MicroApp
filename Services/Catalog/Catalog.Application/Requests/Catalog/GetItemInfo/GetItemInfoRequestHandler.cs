using AutoMapper;
using Catalog.Application.Dto.CatalogItem;
using Catalog.Application.Exceptions;
using Catalog.Application.Interfaces.DataAccess;
using Catalog.Domian.Entities;
using MediatR;

namespace Catalog.Application.Requests.Catalog.GetItemInfo;

public class GetItemInfoRequestHandler : IRequestHandler<GetItemInfoRequest, CatalogItemInfoDto>
{
    private readonly ICatalogDbContext _catalogDb;
    private readonly IMapper _mapper;

    public GetItemInfoRequestHandler(ICatalogDbContext catalogDb, IMapper mapper)
    {
        _catalogDb = catalogDb;
        _mapper = mapper;
    }

    public async Task<CatalogItemInfoDto> Handle(GetItemInfoRequest request, CancellationToken cancellationToken)
    {
        CatalogItem item = await _catalogDb.CatalogItems.FindAsync(request.Id) ??
            throw new EntityNotFoundException(nameof(CatalogItem));

        return _mapper.Map<CatalogItemInfoDto>(item);
    }
}