using IntegrationServices.Models;
using Ordering.Application.IntegrationEvents.Models;

namespace Ordering.Application.IntegrationEvents.Events;

public record BasketCheckoutIntegrationEvent(
    Guid RequestId, Guid UserId, 
    string UserName, CustomerBasket Basket) : IntegrationEvent;