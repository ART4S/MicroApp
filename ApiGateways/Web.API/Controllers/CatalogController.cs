using Microsoft.AspNetCore.Mvc;
using Web.API.Attributes;
using Web.API.Models.Catalog.CatalogBrand;
using Web.API.Models.Catalog.CatalogItem;
using Web.API.Models.Catalog.CatalogType;
using Web.API.Pagination;
using Web.API.Services.Catalog;

namespace Web.API.Controllers;

[Route("/api/v1/catalog")]
[ApiController]
public class CatalogController : ControllerBase
{
    private readonly ICatalogService _catalogService;

    public CatalogController(ICatalogService catalogService)
    {
        _catalogService = catalogService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(PaginationResponse<CatalogItemDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetItems([FromQuery] PaginationRequest request)
    {
        return Ok(await _catalogService.GetItems(request));
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(CatalogItemInfoDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetItem([RequiredNonDefault] Guid id)
    {
        return Ok(await _catalogService.GetItem(id));
    }

    [HttpGet("types")]
    [ProducesResponseType(typeof(CatalogTypeDto[]), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTypes()
    {
        return Ok(await _catalogService.GetTypes());
    }

    [HttpGet("brands")]
    [ProducesResponseType(typeof(CatalogBrandDto[]), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetBrands()
    {
        return Ok(await _catalogService.GetBrands());
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateItem(CatalogItemEditDto item)
    {
        Guid id = await _catalogService.CreateItem(item);

        return CreatedAtAction(nameof(GetItem), new { id }, null);
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> UpdateItem([RequiredNonDefault] Guid id, CatalogItemEditDto item)
    {
        await _catalogService.UpdateItem(id, item);

        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteItem([RequiredNonDefault] Guid id)
    {
        await _catalogService.DeleteItem(id);

        return NoContent();
    }
}
