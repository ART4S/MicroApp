using IntegrationServices.Models;

namespace Payment.API.Integration.Events;

public record PaymentFailedIntegrationEvent(Guid OrderId) : IntegrationEvent;
