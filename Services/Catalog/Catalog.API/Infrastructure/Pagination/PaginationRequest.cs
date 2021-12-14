using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Infrastructure.Pagination;

[BindProperties(SupportsGet = true)]
public record PaginationRequest<TDto> : IRequest<PaginationResponse<TDto>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = Constants.Pagination.MAX_PAGE_SIZE;
}
