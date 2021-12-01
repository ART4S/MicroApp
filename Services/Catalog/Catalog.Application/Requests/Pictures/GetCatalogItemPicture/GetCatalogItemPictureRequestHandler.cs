using Catalog.Application.Dto.Pictures;
using Catalog.Application.Exceptions;
using Catalog.Application.Interfaces.DataAccess;
using MediatR;

namespace Catalog.Application.Requests.Pictures.GetCatalogItemPicture;

public class GetCatalogItemPictureRequestHandler : IRequestHandler<GetCatalogItemPictureRequest, PictureDto>
{
    private readonly IPictureRepository _pictureRepo;

    public GetCatalogItemPictureRequestHandler(IPictureRepository pictureRepo)
    {
        _pictureRepo = pictureRepo;
    }

    public async Task<PictureDto> Handle(GetCatalogItemPictureRequest request, CancellationToken cancellationToken)
    {
        if (!await _pictureRepo.CatalogItemHasPicture(request.ItemId))
            throw new NotFoundException("Picture");

        PictureDto pic = await _pictureRepo.GetCatalogItemPicture(request.ItemId);

        return pic;
    }
}
