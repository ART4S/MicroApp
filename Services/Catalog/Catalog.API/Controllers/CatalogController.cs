using Catalog.API.Application.Models.CatalogBrand;
using Catalog.API.Application.Models.CatalogItem;
using Catalog.API.Application.Models.CatalogType;
using Catalog.API.Application.Requests.Catalog.CreateItem;
using Catalog.API.Application.Requests.Catalog.DeleteItem;
using Catalog.API.Application.Requests.Catalog.GetBrands;
using Catalog.API.Application.Requests.Catalog.GetById;
using Catalog.API.Application.Requests.Catalog.GetItems;
using Catalog.API.Application.Requests.Catalog.GetTypes;
using Catalog.API.Application.Requests.Catalog.UpdateItem;
using Catalog.API.Infrastructure.Attributes;
using Catalog.API.Infrastructure.Pagination;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace Catalog.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Produces(MediaTypeNames.Application.Json)]
public class CatalogController : BaseController
{
    [HttpGet]
    [ProducesResponseType(typeof(PagedResponse<CatalogItemDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetItems([FromQuery] PagedRequest<CatalogItemDto> request)
    {
        var items = await Mediator.Send(new GetItemsRequest());

        return Ok(items.PaginateAsync(request));
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(CatalogItemInfoDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetById([RequiredNonDefault] Guid id)
    {
        return Ok(await Mediator.Send(new GetByIdRequest(id)));
    }

    [HttpGet("types")]
    [ProducesResponseType(typeof(CatalogTypeDto[]), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTypes()
    {
        return Ok(await Mediator.Send(new GetTypesRequest()));
    }

    [HttpGet("brands")]
    [ProducesResponseType(typeof(CatalogBrandDto[]), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetBrands()
    {
        return Ok(await Mediator.Send(new GetBrandsRequest()));
    }

    [HttpPost]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateItem(CatalogItemEditDto item)
    {
        return Ok(await Mediator.Send(new CreateItemRequest(item)));
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> UpdateItem([RequiredNonDefault] Guid id, CatalogItemEditDto item)
    {
        await Mediator.Send(new UpdateItemRequest(id, item));

        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteItem([RequiredNonDefault] Guid id)
    {
        await Mediator.Send(new DeleteItemRequest(id));

        return NoContent();
    }
}
