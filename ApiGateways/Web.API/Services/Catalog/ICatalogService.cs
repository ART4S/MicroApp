using Web.API.Models.Catalog.CatalogBrands;
using Web.API.Models.Catalog.CatalogItems;
using Web.API.Models.Catalog.CatalogTypes;
using Web.API.Models.Catalog.Pictures;
using Web.API.Pagination;

namespace Web.API.Services.Catalog;

public interface ICatalogService
{
    Task<CatalogItemInfoDto> GetItem(Guid id);
    Task<PaginationResponse<CatalogItemDto>> GetItems(PaginationRequest request);
    Task<ICollection<CatalogTypeDto>> GetTypes();
    Task<ICollection<CatalogBrandDto>> GetBrands();
    Task<PictureDto> GetPicture(Guid id);
    Task<Guid> CreateItem(CatalogItemEditDto item);
    Task UpdateItem(Guid id, CatalogItemEditDto item);
    Task DeleteItem(Guid id);
}