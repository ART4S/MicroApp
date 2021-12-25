using Catalog.API.Application.Exceptions;
using Catalog.API.Application.IntegrationEvents.Events;
using Catalog.API.DataAccess;
using Catalog.API.Models;
using IntegrationServices;
using MediatR;

namespace Catalog.API.Application.Requests.Catalog.DeleteItem;

public class DeleteItemRequestHandler : IRequestHandler<DeleteItemRequest>
{
    private readonly ICatalogDbContext _catalogDb;
    private readonly IIntegrationEventService _integrationService;

    public DeleteItemRequestHandler(
        ICatalogDbContext catalogDb,
        IIntegrationEventService integrationService)
    {
        _catalogDb = catalogDb;
        _integrationService = integrationService;
    }

    public async Task<Unit> Handle(DeleteItemRequest request, CancellationToken cancellationToken)
    {
        CatalogItem product = await _catalogDb.Products.GetById(request.ProductId) ??
            throw new EntityNotFoundException(nameof(CatalogItem));

        await _catalogDb.Products.Remove(request.ProductId);

        await _integrationService.Publish(new CatalogItemRemovedIntegrationEvent(
            ItemId: product.Id,
            PictureName: product.PictureName));

        return Unit.Value;
    }
}