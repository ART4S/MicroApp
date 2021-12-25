using Catalog.API.Application.Models.Pictures;

namespace Catalog.API.DataAccess.Repositories;

public interface IPictureRepository
{
    Task<PictureDto?> GetPicture(string pictureName);
    Task RemovePicture(string pictureName);
}
