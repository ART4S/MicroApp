using Web.API.Models.Identity;

namespace Web.API.Services.Identity;

public class UserService : IUserService
{
    public Task<UserDto> GetCurrentUser()
    {
        return Task.FromResult(new UserDto() { Id = new Guid("7f6e4cf9-ac94-4a91-bbe5-e88dcf7a3980"), Name = "Test user" });
    }
}
