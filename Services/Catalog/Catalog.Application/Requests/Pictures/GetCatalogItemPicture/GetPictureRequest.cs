using Catalog.Application.Dto.Pictures;
using MediatR;

namespace Catalog.Application.Requests.Pictures.GetCatalogItemPicture;

public record GetPictureRequest(string ImageName) : IRequest<PictureDto>;
