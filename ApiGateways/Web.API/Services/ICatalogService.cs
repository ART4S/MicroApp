using Web.API.Models.Catalog.CatalogBrands;
using Web.API.Models.Catalog.CatalogItems;
using Web.API.Models.Catalog.CatalogTypes;
using Web.API.Models.Catalog.Pictures;
using Web.API.Pagination;

namespace Web.API.Services;

public interface ICatalogService
{
    Task<CatalogItemInfoDto> GetItem(Guid id);
    Task<PagedResponse<CatalogItemDto>> GetItems(PagedRequest request);
    Task<ICollection<CatalogTypeDto>> GetTypes();
    Task<ICollection<CatalogBrandDto>> GetBrands();
    Task<PictureDto> GetPicture(string pictureName);
    Task<Guid> CreateItem(CatalogItemEditDto item);
    Task UpdateItem(Guid id, CatalogItemEditDto item);
    Task DeleteItem(Guid id);
}