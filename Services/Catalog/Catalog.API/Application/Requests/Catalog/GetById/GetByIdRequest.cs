using Catalog.API.Application.Models.CatalogItem;
using MediatR;

namespace Catalog.API.Application.Requests.Catalog.GetById;

public record GetByIdRequest(Guid ProductId) : IRequest<CatalogItemInfoDto>;
