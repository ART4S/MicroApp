using Identity.API.Models.Entities;
using Microsoft.AspNetCore.Authentication;

namespace Identity.API.Services;

public interface IAuthService
{
    Task<User?> FindUserById(string userId);

    Task<User?> FindUserByName(string userName);

    Task<bool> ValidateUserCredentials(User user, string password);

    Task SignIn(User user, AuthenticationProperties authProps = null, string authMethod = null);

    Task SignOut(User user, string authScheme);
}
