using Catalog.Application.Dto.Pictures;

namespace Catalog.Application.Interfaces.DataAccess;

public interface IPictureRepository
{
    Task<bool> CatalogItemHasPicture(Guid itemId);
    Task<PictureDto> GetCatalogItemPicture(Guid itemId);
}
