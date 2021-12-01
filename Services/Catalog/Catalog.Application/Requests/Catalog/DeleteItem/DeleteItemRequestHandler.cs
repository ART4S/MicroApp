using Catalog.Application.Exceptions;
using Catalog.Application.Integration.Events;
using Catalog.Application.Interfaces.DataAccess;
using Catalog.Domian.Entities;
using IntegrationServices;
using MediatR;

namespace Catalog.Application.Requests.Catalog.DeleteItem;

public class DeleteItemRequestHandler : AsyncRequestHandler<DeleteItemRequest>
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

    protected override async Task Handle(DeleteItemRequest request, CancellationToken cancellationToken)
    {
        CatalogItem? item = await _catalogDb.CatalogItems.FindAsync(request.Id);

        if (item is null)
            throw new NotFoundException(nameof(CatalogItem));

        _catalogDb.CatalogItems.Remove(item);

        await _catalogDb.SaveChanges();

        await _integrationService.Save(new CatalogItemRemovedIntegrationEvent(item.Id));
    }
}