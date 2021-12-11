using Catalog.Application.Dto.Pictures;
using Catalog.Application.Requests.Pictures.GetCatalogItemPicture;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Controllers;

[ApiController]
public class PicturesController : BaseController
{
    [HttpGet("api/v1/catalog/items/{itemId:guid}/picture")]
    public async Task<IActionResult> GetCatalogItemPicture(Guid itemId)
    {
        if (itemId == Guid.Empty) return BadRequest();

        PictureDto pic = await Mediator.Send(new GetCatalogItemPictureRequest(itemId));

        string contentType = GetContentTypeByFileExtension(pic.Name);

        return File(pic.Data, contentType);
    }

    private string GetContentTypeByFileExtension(string fileName)
    {
        return Path.GetExtension(fileName).ToLower() switch
        {
            ".png" => "image/png",
            ".svg" => "image/svg+xml",
            string x when new[] { ".jpg", ".jpeg" }.Contains(x) => "image/jpeg",
            _ => "application/octet-stream"
        };
    }
}
