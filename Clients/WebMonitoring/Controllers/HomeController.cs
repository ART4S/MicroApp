using Microsoft.AspNetCore.Mvc;

[Route("")]
public class HomeController : ControllerBase
{
    public IActionResult Index()
    {
        return Redirect("~/hc-ui");
    }
}
