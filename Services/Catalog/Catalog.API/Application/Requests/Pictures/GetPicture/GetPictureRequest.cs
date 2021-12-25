using Catalog.API.Application.Models.Pictures;
using MediatR;

namespace Catalog.API.Application.Requests.Pictures.GetPicture;

public record GetPictureRequest(string PictureName) : IRequest<PictureDto>;
