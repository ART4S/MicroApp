using IntegrationServices.Models;

namespace Ordering.Application.Integration.Events;

public record PaymentFailedIntegrationEvent(Guid OrderId) : IntegrationEvent;
