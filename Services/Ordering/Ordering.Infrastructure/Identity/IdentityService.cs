using Ordering.Application.Model.Identity;
using Ordering.Application.Services.Identity;

namespace Ordering.Infrastructure.Identity;

public class IdentityService : IIdentityService
{
    public Task<UserDto> GetCurrentUser()
    {
        return Task.FromResult(new UserDto() { Id = new Guid("7f6e4cf9-ac94-4a91-bbe5-e88dcf7a3980"), Name = "Test user" });
    }

    public Task<UserDto> GetUser(Guid userId)
    {
        return Task.FromResult(new UserDto() { Id = userId, Name = "Test user" });
    }
}
