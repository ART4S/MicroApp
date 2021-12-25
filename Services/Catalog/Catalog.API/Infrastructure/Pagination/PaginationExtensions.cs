using Catalog.API.Application.Exceptions;

namespace Catalog.API.Infrastructure.Pagination;

static class PaginationExtentions
{
    public static PagedResponse<TDto> PaginateAsync<TDto>(
        this IEnumerable<TDto> source,
        PagedRequest<TDto> request)
    {
        if (request.PageNumber < 1)
            throw new InvalidRequestException("Page number must be greather than 0");
        if (request.PageSize < 1)
            throw new InvalidRequestException("Page size must me greather than 0");
        if (request.PageNumber > Constants.Pagination.MAX_PAGE_SIZE)
            throw new InvalidRequestException($"Max page must be less or equal {Constants.Pagination.MAX_PAGE_SIZE}");

        int totalItemsCount = source.Count();
        int totalPages = (int)Math.Ceiling(totalItemsCount / (double)request.PageSize);
        int currentPage = Math.Min(request.PageNumber, totalPages);
        IList<TDto> data = source
            .Skip((currentPage - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToList();
        int pageSize = data.Count;

        return new(pageSize, currentPage, totalPages, totalItemsCount, data);
    }
}