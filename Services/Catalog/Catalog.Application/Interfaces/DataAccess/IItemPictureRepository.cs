using Catalog.Application.Dto.Pictures;

namespace Catalog.Application.Interfaces.DataAccess;

public interface IItemPictureRepository
{
    Task<PictureDto?> GetPicture(Guid productId);
    Task RemovePictureByName(string pictureName);
}
