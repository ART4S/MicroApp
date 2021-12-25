using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Web.API.Attributes;
using Web.API.Models.Catalog.CatalogBrands;
using Web.API.Models.Catalog.CatalogItems;
using Web.API.Models.Catalog.CatalogTypes;
using Web.API.Pagination;
using Web.API.Services;

namespace Web.API.Controllers;

[Route("/api/catalog")]
[ApiController]
public class CatalogController : BaseController
{
    private readonly ICatalogService _catalogService;

    public CatalogController(ICatalogService catalogService)
    {
        _catalogService = catalogService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(PagedResponse<CatalogItemDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetItems([FromQuery] PagedRequest request)
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

    [HttpGet("pictures/{pictureName}")]
    public async Task<IActionResult> GetPicture([Required] string pictureName)
    {
        var file = await _catalogService.GetPicture(pictureName);

        return File(file.Content, file.ContentType);
    }

    [HttpPost]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateItem(CatalogItemEditDto item)
    {
        return Created(await _catalogService.CreateItem(item));
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