using IntegrationServices.Models;
using Ordering.SignalR.IntegrationEvents.Models;

namespace Ordering.SignalR.IntegrationEvents.Events;

public record OrderCancelledIntegrationEvent(Order Order) : IntegrationEvent;
