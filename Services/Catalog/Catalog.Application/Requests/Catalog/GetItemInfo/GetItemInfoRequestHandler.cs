using AutoMapper;
using Catalog.Application.Dto.CatalogItem;
using Catalog.Application.Exceptions;
using Catalog.Application.Services;
using Catalog.Domian.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

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
        CatalogItem item = await _catalogDb.CatalogItems
            .Include(x => x.Brand)
            .Include(x => x.Type)
            .AsNoTracking()
            .SingleOrDefaultAsync(x => x.Id == request.Id) ??
        throw new EntityNotFoundException(nameof(CatalogItem));

        return _mapper.Map<CatalogItemInfoDto>(item);
    }
}