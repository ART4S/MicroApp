using IntegrationServices.Models;
using Ordering.SignalR.IntegrationEvents.Models;

namespace Ordering.SignalR.IntegrationEvents.Events;

public record OrderAcceptedIntegrationEvent(Order Order) : IntegrationEvent;
