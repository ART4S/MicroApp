using IntegrationServices.Models;
using Payment.API.IntegrationEvents.Models;

namespace Payment.API.IntegrationEvents.Events;

public record OrderAcceptedIntegrationEvent(AcceptedOrder Order) : IntegrationEvent;
