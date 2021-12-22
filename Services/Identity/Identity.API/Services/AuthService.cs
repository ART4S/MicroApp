using Identity.API.Models.Entities;
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

    public Task<User?> FindUserByName(string userName)
    {
        return _userManager.FindByNameAsync(userName);
    }

    public Task<bool> ValidateUserCredentials(User user, string password)
    {
        return _userManager.CheckPasswordAsync(user, password);
    }

    public Task SignIn(User user, AuthenticationProperties authProps)
    {
        return _signInManager.SignInAsync(user, authProps);
    }

    public Task SignOut()
    {
        return _signInManager.SignOutAsync();
    }
}