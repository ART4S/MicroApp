using Basket.API.Models;
using IntegrationServices.Models;

namespace Basket.API.Infrastructure.IntegrationEvents.Events;

public record BasketCheckoutIntegrationEvent(
    string RequestId, string UserId, 
    string UserName, CustomerBasket Basket) : IntegrationEvent;
