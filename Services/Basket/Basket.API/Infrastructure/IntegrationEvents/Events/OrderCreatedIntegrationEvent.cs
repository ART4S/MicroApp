using Basket.API.Infrastructure.IntegrationEvents.Models;
using IntegrationServices.Models;

namespace Basket.API.Infrastructure.IntegrationEvents.Events;

public record OrderCreatedIntegrationEvent(CreatedOrder Order) : IntegrationEvent;
