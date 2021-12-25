using Web.API.Models.Identity;

namespace Web.API.Services;

public class UserService : IUserService
{
    private readonly HttpContext _httpContext;

    public UserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContext = httpContextAccessor.HttpContext ?? throw new NullReferenceException(nameof(httpContextAccessor.HttpContext));
    }

    public Task<User> GetCurrentUser()
    {
        return Task.FromResult(new User
        (
            Id: new Guid(_httpContext.User.Claims.Single(x => x.Type == "sub").Value),
            Name: _httpContext.User.Claims.Single(x => x.Type == "name").Value
        ));
    }
}
