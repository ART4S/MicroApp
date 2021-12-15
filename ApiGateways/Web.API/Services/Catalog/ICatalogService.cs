using Web.API.Models.Catalog.CatalogBrand;
using Web.API.Models.Catalog.CatalogItem;
using Web.API.Models.Catalog.CatalogType;
using Web.API.Pagination;

namespace Web.API.Services.Catalog;

public interface ICatalogService
{
    Task<CatalogItemInfoDto> GetItem(Guid id);
    Task<PaginationResponse<CatalogItemDto>> GetItems(PaginationRequest request);
    Task<ICollection<CatalogTypeDto>> GetTypes();
    Task<ICollection<CatalogBrandDto>> GetBrands();
    Task<Guid> CreateItem(CatalogItemEditDto item);
    Task UpdateItem(Guid id, CatalogItemEditDto item);
    Task DeleteItem(Guid id);
}