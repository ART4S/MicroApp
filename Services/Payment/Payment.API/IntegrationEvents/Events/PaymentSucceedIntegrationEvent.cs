using IntegrationServices.Models;

namespace Payment.API.IntegrationEvents.Events;

public record PaymentSucceedIntegrationEvent(Guid OrderId) : IntegrationEvent;
