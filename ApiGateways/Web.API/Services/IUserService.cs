using Web.API.Models.Identity;

namespace Web.API.Services;

public interface IUserService
{
    Task<User> GetCurrentUser();
}
