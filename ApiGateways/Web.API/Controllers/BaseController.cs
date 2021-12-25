using Microsoft.AspNetCore.Mvc;

namespace Web.API.Controllers;

public abstract class BaseController : ControllerBase
{
    protected IActionResult Created(object value)
    {
        return StatusCode(StatusCodes.Status201Created, value);
    }
}
