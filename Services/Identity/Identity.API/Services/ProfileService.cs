using Identity.API.Models.Entities;
using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Identity.API.Services;

public class ProfileService : IProfileService
{
    private readonly UserManager<User> _userManager;

    public ProfileService(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public async Task GetProfileDataAsync(ProfileDataRequestContext context)
    {
        ClaimsPrincipal subject = context.Subject ?? throw new ArgumentNullException(nameof(context.Subject));

        string userId = subject.Claims.Single(x => x.Type == JwtClaimTypes.Subject).Value;

        User user = await _userManager.FindByIdAsync(userId) ??
            throw new ArgumentException("User not found");

        context.IssuedClaims = new()
        {
            new(JwtClaimTypes.Name, user.UserName),
        };
    }

    public async Task IsActiveAsync(IsActiveContext context)
    {
        ClaimsPrincipal subject = context.Subject ?? throw new ArgumentNullException(nameof(context.Subject));

        string userId = subject.Claims.Single(x => x.Type == JwtClaimTypes.Subject).Value;

        User? user = await _userManager.FindByIdAsync(userId);

        context.IsActive = user is not null;
    }
}