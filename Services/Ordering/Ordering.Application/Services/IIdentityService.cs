using Ordering.Application.Dto;

namespace Ordering.Application.Services.Identity;

public interface IIdentityService
{
    Task<UserDto?> GetUser(Guid userId);
}
