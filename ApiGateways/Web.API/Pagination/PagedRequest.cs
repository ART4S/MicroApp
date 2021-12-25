using Microsoft.AspNetCore.Mvc;

namespace Web.API.Pagination;

[BindProperties(SupportsGet = true)]
public record PagedRequest
{
    [BindProperty(Name = "page_number")]
    public int PageNumber { get; init; }

    [BindProperty(Name = "page_size")]
    public int PageSize { get; init; }
}
