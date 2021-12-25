using Microsoft.AspNetCore.Mvc;

namespace Web.API.Controllers;

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
