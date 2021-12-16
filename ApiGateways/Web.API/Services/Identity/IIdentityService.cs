using Web.API.Models.Identity;

namespace Web.API.Services.Identity;

public interface IIdentityService
{
    Task<UserDto> GetCurrentUser();
}
