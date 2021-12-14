using IntegrationServices.Models;

namespace Ordering.Application.Integration.Events;

public record OrderCreatedIntegrationEvent(Guid BuyerId, Guid OrderId) : IntegrationEvent;