using IntegrationServices.Models;

namespace Ordering.Application.Integration.Events;

public record PaymentSucceedIntegrationEvent(Guid OrderId) : IntegrationEvent;
