using IntegrationServices.Models;

namespace Payment.API.IntegrationEvents.Events;

public record PaymentFailedIntegrationEvent(Guid OrderId) : IntegrationEvent;
