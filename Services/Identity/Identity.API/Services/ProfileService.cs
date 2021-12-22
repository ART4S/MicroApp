using Identity.API.Models.Entities;
using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.JsonWebTokens;
using System.Security.Claims;

namespace Identity.API.Services;

// TODO: add ICurrentTime, add UserNotFoundException
public class ProfileService : IProfileService
{
    private readonly UserManager<User> _userManager;

    public ProfileService(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    async public Task GetProfileDataAsync(ProfileDataRequestContext context)
    {
        ClaimsPrincipal subject = context.Subject ?? throw new ArgumentNullException(nameof(context.Subject));

        string userId = subject.Claims.Single(x => x.Type == JwtClaimTypes.Subject).Value;

        User user = await _userManager.FindByIdAsync(userId) ??
            throw new ArgumentException("User not found");

        await _userManager.GetClaimsAsync(user);

        context.IssuedClaims = GetClaimsFromUser(user);
    }

    async public Task IsActiveAsync(IsActiveContext context)
    {
        ClaimsPrincipal subject = context.Subject ?? throw new ArgumentNullException(nameof(context.Subject));

        string userId = subject.Claims.Single(x => x.Type == JwtClaimTypes.Subject).Value;

        User? user = await _userManager.FindByIdAsync(userId);

        context.IsActive = user is not null;
    }

    private List<Claim> GetClaimsFromUser(User user)
    {
        return new()
        {
            new(JwtClaimTypes.Subject, user.Id.ToString()),
            new(JwtClaimTypes.PreferredUserName, user.UserName),
            new(JwtRegisteredClaimNames.UniqueName, user.UserName)
        };
    }
}