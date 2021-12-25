using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Infrastructure.Pagination;

[BindProperties(SupportsGet = true)]
public record PagedRequest<TDto> : IRequest<PagedResponse<TDto>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = Constants.Pagination.MAX_PAGE_SIZE;
}
