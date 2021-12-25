using AutoMapper;
using Catalog.API.Application.Exceptions;
using Catalog.API.DataAccess;
using Catalog.API.Models;
using MediatR;

namespace Catalog.API.Application.Requests.Catalog.CreateItem;

public class CreateItemRequestHandler : IRequestHandler<CreateItemRequest, Guid>
{
    private readonly ICatalogDbContext _catalogDb;
    private readonly IMapper _mapper;

    public CreateItemRequestHandler(ICatalogDbContext catalogDb, IMapper mapper)
    {
        _catalogDb = catalogDb;
        _mapper = mapper;
    }

    public async Task<Guid> Handle(CreateItemRequest request, CancellationToken cancellationToken)
    {
        CatalogItem product = _mapper.Map<CatalogItem>(request.Product);

        product.Brand = await _catalogDb.CatalogBrands.GetById(request.Product.BrandId) ??
            throw new EntityNotFoundException(nameof(CatalogBrand));

        product.Type = await _catalogDb.CatalogTypes.GetById(request.Product.TypeId) ??
            throw new EntityNotFoundException(nameof(CatalogType));

        await _catalogDb.Products.Create(product);

        return product.Id;
    }
}