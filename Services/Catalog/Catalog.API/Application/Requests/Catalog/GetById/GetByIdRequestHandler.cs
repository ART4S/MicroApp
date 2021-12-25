using AutoMapper;
using Catalog.API.Application.Exceptions;
using Catalog.API.Application.Models.CatalogItem;
using Catalog.API.DataAccess;
using Catalog.API.Models;
using MediatR;

namespace Catalog.API.Application.Requests.Catalog.GetById;

public class GetByIdRequestHandler : IRequestHandler<GetByIdRequest, CatalogItemInfoDto>
{
    private readonly ICatalogDbContext _catalogDb;
    private readonly IMapper _mapper;

    public GetByIdRequestHandler(ICatalogDbContext catalogDb, IMapper mapper)
    {
        _catalogDb = catalogDb;
        _mapper = mapper;
    }

    public async Task<CatalogItemInfoDto> Handle(GetByIdRequest request, CancellationToken cancellationToken)
    {
        CatalogItem product = await _catalogDb.Products.GetById(request.ProductId) ??
            throw new EntityNotFoundException(nameof(CatalogItem));

        return _mapper.Map<CatalogItemInfoDto>(product);
    }
}