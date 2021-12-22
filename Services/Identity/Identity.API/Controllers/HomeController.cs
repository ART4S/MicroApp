using IdentityServer4.Services;
using Microsoft.AspNetCore.Mvc;

namespace Identity.API.Controllers;

[Route("/api/home")]
[ApiController]
public class HomeController : ControllerBase
{
    [HttpGet("error")]
    public async Task<IActionResult> Error(string errorId, [FromServices] IIdentityServerInteractionService interaction)
    {
        var ctx = await interaction.GetErrorContextAsync(errorId);

        return Ok(ctx);
    }
}
