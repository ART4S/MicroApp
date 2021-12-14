using IntegrationServices.Models;

namespace Payment.API.Integration.Events;

public record PaymentSucceedIntegrationEvent(Guid OrderId) : IntegrationEvent;
