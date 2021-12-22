using Ordering.API.Models;

namespace Ordering.API.Services;

public class BuyerService : IBuyerService
{
    private readonly HttpContext _httpContext;

    public BuyerService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContext = httpContextAccessor.HttpContext ?? throw new NullReferenceException(nameof(httpContextAccessor.HttpContext));
    }

    public Task<BuyerInfo> GetCurrentBuyer()
    {
        return Task.FromResult(new BuyerInfo
        (
            Id: new Guid(_httpContext.User.Claims.Single(x => x.Type == "sub").Value),
            Name: _httpContext.User.Claims.Single(x => x.Type == "name").Value
        ));
    }
}
