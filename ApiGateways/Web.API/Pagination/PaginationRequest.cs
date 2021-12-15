using Microsoft.AspNetCore.Mvc;

namespace Web.API.Pagination;

[BindProperties(SupportsGet = true)]
public record PaginationRequest
{
    public int PageNumber { get; init; }
    public int PageSize { get; init; }
}
