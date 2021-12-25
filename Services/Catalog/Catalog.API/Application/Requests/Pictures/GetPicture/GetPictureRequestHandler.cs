using Catalog.API.Application.Exceptions;
using Catalog.API.Application.Models.Pictures;
using Catalog.API.DataAccess.Repositories;
using MediatR;

namespace Catalog.API.Application.Requests.Pictures.GetPicture;

public class GetPictureRequestHandler : IRequestHandler<GetPictureRequest, PictureDto>
{
    private readonly IPictureRepository _pictureRepo;

    public GetPictureRequestHandler(IPictureRepository pictureRepo)
    {
        _pictureRepo = pictureRepo;
    }

    public async Task<PictureDto> Handle(GetPictureRequest request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.PictureName))
            throw new EntityNotFoundException("Picture");

        PictureDto pic = await _pictureRepo.GetPicture(request.PictureName) ??
            throw new EntityNotFoundException("Picture");

        return pic;
    }
}
