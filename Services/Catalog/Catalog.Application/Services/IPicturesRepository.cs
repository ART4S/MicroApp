using Catalog.Application.Dto.Pictures;

namespace Catalog.Application.Services;

public interface IPicturesRepository
{
    Task<PictureDto?> GetPicture(string pictureName);
    Task RemovePicture(string pictureName);
}
