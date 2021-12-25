using AutoMapper;
using Catalog.API.Application.Exceptions;
using Catalog.API.Application.IntegrationEvents.Events;
using Catalog.API.DataAccess;
using Catalog.API.Models;
using IntegrationServices;
using MediatR;

namespace Catalog.API.Application.Requests.Catalog.UpdateItem;

public class UpdateItemRequestHandler : IRequestHandler<UpdateItemRequest, Unit>
{
    private readonly ICatalogDbContext _catalogDb;
    private readonly IIntegrationEventService _integrationService;
    private readonly IMapper _mapper;

    public UpdateItemRequestHandler(
        ICatalogDbContext catalogDb,
        IIntegrationEventService integrationService,
        IMapper mapper)
    {
        _catalogDb = catalogDb;
        _integrationService = integrationService;
        _mapper = mapper;
    }

    public async Task<Unit> Handle(UpdateItemRequest request, CancellationToken cancellationToken)
    {
        CatalogItem product = await _catalogDb.Products.GetById(request.ProductId) ??
            throw new EntityNotFoundException(nameof(CatalogItem));

        if (product.Brand.Id != request.Product.BrandId)
            product.Brand = await _catalogDb.CatalogBrands.GetById(request.Product.BrandId) ??
                throw new EntityNotFoundException(nameof(CatalogBrand));

        if (product.Type.Id != request.Product.TypeId)
            product.Type = await _catalogDb.CatalogTypes.GetById(request.Product.TypeId) ??
                throw new EntityNotFoundException(nameof(CatalogType));

        decimal? oldPrice = product.Price;
        decimal? newPrice = request.Product.Price;

        _mapper.Map(request.Product, product);

        await _catalogDb.Products.Update(product);

        if (oldPrice != newPrice)
        {
            await _integrationService.Publish(new CatalogItemPriceChangedIntegrationEvent
            (
                ItemId: product.Id,
                NewPrice: newPrice
            ));
        }

        return Unit.Value;
    }
}