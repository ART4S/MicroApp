using Identity.API.Models.Entities;
using Identity.API.Models.ViewModels;
using Identity.API.Services;
using IdentityServer4;
using IdentityServer4.Events;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using IdentityServer4.Test;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Identity.API.Controllers;

[Route("/api/auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IIdentityServerInteractionService _interaction;
    //private readonly IEventService _events;

    public AuthController(
      IIdentityServerInteractionService interaction,
      IAuthService authService)
    {
        _authService = authService;
        _interaction = interaction;
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginVm loginVm)
    {
        User? user = await _authService.FindUserByName(loginVm.UserName);

        if (user is null || !await _authService.ValidateUserCredentials(user, loginVm.Password))
            return BadRequest("Invalid credentials");

        //await _authService.SignIn(user, );

        await _authService.SignIn(user);

        return Ok();
    }

    [Authorize]
    [HttpGet("test")]
    public async Task<IActionResult> Test(string? id_token)
    {
        return Ok(
        new 
        {
            claims = User.Claims.Select(x => $"{x.Type} - {x.Value}"),
            id_token
        });
    }
}
