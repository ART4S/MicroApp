using AutoMapper;
using Catalog.Application.Interfaces.DataAccess;
using Catalog.Domian.Entities;
using MediatR;

namespace Catalog.Application.Requests.Catalog.CreateItem;

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
        CatalogItem item = _mapper.Map<CatalogItem>(request.Item);

        await _catalogDb.CatalogItems.AddAsync(item);

        await _catalogDb.SaveChanges();

        return item.Id;
    }
}
