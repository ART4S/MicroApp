using IntegrationServices.Models;

namespace Ordering.Application.IntegrationEvents.Events;

public record PaymentFailedIntegrationEvent(Guid OrderId) : IntegrationEvent;
