using Catalog.Application.Dto.Pictures;
using Catalog.Application.Exceptions;
using Catalog.Application.Services;
using MediatR;

namespace Catalog.Application.Requests.Pictures.GetCatalogItemPicture;

public class GetPictureRequestHandler : IRequestHandler<GetPictureRequest, PictureDto>
{
    private readonly IPicturesRepository _pictureRepo;

    public GetPictureRequestHandler(IPicturesRepository pictureRepo)
    {
        _pictureRepo = pictureRepo;
    }

    public async Task<PictureDto> Handle(GetPictureRequest request, CancellationToken cancellationToken)
    {
        PictureDto pic = await _pictureRepo.GetPicture(request.ImageName) ??
            throw new EntityNotFoundException("Picture");

        return pic;
    }
}
