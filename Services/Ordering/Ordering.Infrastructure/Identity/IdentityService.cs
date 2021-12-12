using Ordering.Application.Dto;
using Ordering.Application.Services.Identity;

namespace Ordering.Infrastructure.Identity;

public class IdentityService : IIdentityService
{
    public Task<UserDto> GetUser(Guid userId)
    {
        return Task.FromResult(new UserDto() { Id = userId, Name = "Test user" });
    }
}
