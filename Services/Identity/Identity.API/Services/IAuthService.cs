using Identity.API.Models.Entities;
using Microsoft.AspNetCore.Authentication;

namespace Identity.API.Services;

public interface IAuthService
{
    Task<User?> FindUserByName(string userName);

    Task<bool> ValidateUserCredentials(User user, string password);

    Task SignIn(User user, AuthenticationProperties authProps);

    Task SignOut();
}
