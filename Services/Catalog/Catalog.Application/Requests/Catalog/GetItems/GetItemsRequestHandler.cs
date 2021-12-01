using AutoMapper;
using AutoMapper.QueryableExtensions;
using Catalog.Application.Dto.CatalogItem;
using Catalog.Application.Interfaces.DataAccess;
using MediatR;

namespace Catalog.Application.Requests.Catalog.GetItems
{
    public class GetItemsRequestHandler : IRequestHandler<GetItemsRequest, IQueryable<CatalogItemDto>>
    {
        private readonly ICatalogDbContext _catalogDb;
        private readonly IMapper _mapper;

        public GetItemsRequestHandler(ICatalogDbContext catalogDb, IMapper mapper)
        {
            _catalogDb = catalogDb;
            _mapper = mapper;
        }

        public Task<IQueryable<CatalogItemDto>> Handle(GetItemsRequest request, CancellationToken cancellationToken)
        {
            return Task.FromResult(_catalogDb.CatalogItems.ProjectTo<CatalogItemDto>(_mapper.ConfigurationProvider));
        }
    }
}
