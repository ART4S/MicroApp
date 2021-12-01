namespace Catalog.API.Infrastructure.Pagination;

public record PagedResponse<TDto>(
    int PageSize, int CurrentPage, 
    int TotalPages, int TotalItemsCount, 
    IList<TDto> Data);
