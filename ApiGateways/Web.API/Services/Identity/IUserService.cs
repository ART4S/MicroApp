using Web.API.Models.Identity;

namespace Web.API.Services.Identity;

public interface IUserService
{
    Task<UserDto> GetCurrentUser();
}
