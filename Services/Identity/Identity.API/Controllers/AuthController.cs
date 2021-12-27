using Identity.API.Models.Entities;
using Identity.API.Models.ViewModels;
using Identity.API.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Identity.API.Controllers;

[Route("/api/auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly ILogger _logger;
    private readonly IAuthService _authService;
    private readonly ICurrentTime _currentTime;

    public AuthController(
        ILogger<AuthController> logger,
        IAuthService authService, 
        ICurrentTime currentTime)
    {
        _logger = logger;
        _authService = authService;
        _currentTime = currentTime;
    }

    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Login(LoginVm loginVm)
    {
        _logger.LogInformation("Attempt to login for user {User}", loginVm.UserName);

        User? user = await _authService.FindUserByName(loginVm.UserName);

        if (user is null || !await _authService.ValidateUserCredentials(user, loginVm.Password))
            return BadRequest("Invalid credentials");

        AuthenticationProperties props = new()
        {
            AllowRefresh = true,
            IsPersistent = true,
        };

        if (loginVm.RememberMe)
            props.ExpiresUtc = _currentTime.Now.AddDays(365);

        await _authService.SignIn(user, props);

        _logger.LogInformation("User {User} logged in", loginVm.UserName);

        return Ok();
    }

    // For testing purposes while spa client is missing
    [Authorize]
    [HttpGet("callback")]
    [ApiExplorerSettings(IgnoreApi = true)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Callback(
        string code, 
        string scope, 
        [FromQuery(Name ="session_state")] string session)
    {
        return Ok(new 
        {
            code,
            scope,
            session,
            claims = User.Claims.Select(x => $"{x.Type} - {x.Value}"),
        });
    }
}