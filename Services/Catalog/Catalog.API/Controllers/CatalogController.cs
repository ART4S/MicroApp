using Catalog.API.Infrastructure.Attributes;
using Catalog.API.Infrastructure.Pagination;
using Catalog.Application.Dto.CatalogBrand;
using Catalog.Application.Dto.CatalogItem;
using Catalog.Application.Dto.CatalogType;
using Catalog.Application.Requests.Catalog.CreateItem;
using Catalog.Application.Requests.Catalog.DeleteItem;
using Catalog.Application.Requests.Catalog.GetBrands;
using Catalog.Application.Requests.Catalog.GetItemInfo;
using Catalog.Application.Requests.Catalog.GetItems;
using Catalog.Application.Requests.Catalog.GetTypes;
using Catalog.Application.Requests.Catalog.UpdateItem;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace Catalog.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Produces(MediaTypeNames.Application.Json)]
public class CatalogController : BaseController
{
    [HttpGet]
    [ProducesResponseType(typeof(PaginationResponse<CatalogItemDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetItems([FromQuery] PaginationRequest<CatalogItemDto> request)
    {
        var items = await Mediator.Send(new GetItemsRequest());

        return Ok(await items.PaginateAsync(request));
    }

    [HttpGet("{id:guid}/info")]
    [ProducesResponseType(typeof(CatalogItemInfoDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetItemInfo([RequiredNonDefault] Guid id)
    {
        return Ok(await Mediator.Send(new GetItemInfoRequest(id)));
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
