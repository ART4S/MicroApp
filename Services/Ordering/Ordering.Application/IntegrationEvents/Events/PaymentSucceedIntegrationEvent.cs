using IntegrationServices.Models;

namespace Ordering.Application.IntegrationEvents.Events;

public record PaymentSucceedIntegrationEvent(Guid OrderId) : IntegrationEvent;
