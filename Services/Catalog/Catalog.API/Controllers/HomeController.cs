using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Controllers;

[Route("")]
[ApiExplorerSettings(IgnoreApi = true)]
public class HomeController : BaseController
{
    [HttpGet]
    public IActionResult Index()
    {
        return Redirect("~/swagger");
    }
}
