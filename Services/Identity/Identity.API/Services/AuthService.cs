using Identity.API.Models.Entities;
using IdentityServer4;
using IdentityServer4.Configuration;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;

namespace Identity.API.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;

    public AuthService(UserManager<User> userManager, SignInManager<User> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public Task<User?> FindUserById(string userId)
    {
         return _userManager.FindByIdAsync(userId);
    }

    public Task<User?> FindUserByName(string userName)
    {
        return _userManager.FindByNameAsync(userName);
    }

    public Task<bool> ValidateUserCredentials(User user, string password)
    {
        return _userManager.CheckPasswordAsync(user, password);
    }

    public async Task SignIn(User user, AuthenticationProperties authProps = null, string authMethod = null)
    {
        //// issue authentication cookie with subject ID and username
        //IdentityServerUser identityUser = new(user.Id.ToString());

        //await HttpContext.SignInAsync(identityUser, props);
        //new IdentityUser() { }
        //return _signInManager.SignInAsync();
        await _signInManager.SignInAsync(user, isPersistent: true);
    }

    public Task SignOut(User user, string authScheme)
    {
        throw new NotImplementedException();
    }
}