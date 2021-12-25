using Catalog.API.Application.Models.Pictures;
using Catalog.API.Application.Requests.Pictures.GetPicture;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class PicturesController : BaseController
{
    [HttpGet("{pictureName}")]
    public async Task<IActionResult> GetPicture(string pictureName)
    {
        PictureDto pic = await Mediator.Send(new GetPictureRequest(pictureName));

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
