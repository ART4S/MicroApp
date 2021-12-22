using Ordering.API.Models;

namespace Ordering.API.Services;

public interface IBuyerService
{
    Task<BuyerInfo> GetCurrentBuyer();
}
