using Ordering.Application.Model.Identity;

namespace Ordering.Application.Services.Identity;

public interface IIdentityService
{
    Task<UserDto?> GetUser(Guid userId);

    Task<UserDto> GetCurrentUser();
}
