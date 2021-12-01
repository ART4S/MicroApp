using Catalog.Application.Dto.CatalogItem;
using MediatR;

namespace Catalog.Application.Requests.Catalog.GetItemInfo;

public record GetItemInfoRequest(Guid Id) : IRequest<CatalogItemInfoDto>;
