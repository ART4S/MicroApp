using Catalog.Application.Dto.Pictures;
using Catalog.Application.Exceptions;
using Catalog.Application.Services.DataAccess;
using MediatR;

namespace Catalog.Application.Requests.Pictures.GetCatalogItemPicture;

public class GetCatalogItemPictureRequestHandler : IRequestHandler<GetCatalogItemPictureRequest, PictureDto>
{
    private readonly IItemPictureRepository _pictureRepo;

    public GetCatalogItemPictureRequestHandler(IItemPictureRepository pictureRepo)
    {
        _pictureRepo = pictureRepo;
    }

    public async Task<PictureDto> Handle(GetCatalogItemPictureRequest request, CancellationToken cancellationToken)
    {
        PictureDto pic = await _pictureRepo.GetPicture(request.ItemId) ??
            throw new EntityNotFoundException("Picture");

        return pic;
    }
}
