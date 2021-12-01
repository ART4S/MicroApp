using Catalog.Application.Dto.Pictures;
using MediatR;

namespace Catalog.Application.Requests.Pictures.GetCatalogItemPicture;

public record GetCatalogItemPictureRequest(Guid ItemId) : IRequest<PictureDto>;
