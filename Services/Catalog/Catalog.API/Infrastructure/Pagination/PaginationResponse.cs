namespace Catalog.API.Infrastructure.Pagination;

public record PaginationResponse<TDto>(
    int PageSize, int CurrentPage, 
    int TotalPages, int TotalItemsCount, 
    IList<TDto> Data);
