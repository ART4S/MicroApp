using AutoMapper;
using Catalog.Application.Exceptions;
using Catalog.Application.Integration.Events;
using Catalog.Application.Services;
using Catalog.Domian.Entities;
using IntegrationServices;
using MediatR;

namespace Catalog.Application.Requests.Catalog.UpdateItem;

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
        CatalogItem item = await _catalogDb.CatalogItems.FindAsync(request.Id) ??
            throw new EntityNotFoundException(nameof(CatalogItem));

        decimal? oldPrice = item.Price;
        decimal? newPrice = request.Item.Price;

        if (oldPrice != newPrice)
        {
            await _integrationService.Save(new CatalogItemPriceChangedIntegrationEvent
            (
                ItemId: item.Id,
                NewPrice: newPrice
            ));
        }

        _mapper.Map(request.Item, item);

        await _catalogDb.SaveChangesAsync();
        
        return Unit.Value;
    }
}
