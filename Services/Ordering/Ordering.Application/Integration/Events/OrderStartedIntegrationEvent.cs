using IntegrationServices.Model;

namespace Ordering.Application.Integration.Events;

public record OrderStartedIntegrationEvent(Guid BuyerId, Guid OrderId) : IntegrationEvent;