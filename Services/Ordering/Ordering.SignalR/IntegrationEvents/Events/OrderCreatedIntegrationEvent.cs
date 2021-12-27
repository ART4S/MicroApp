using IntegrationServices.Models;
using Ordering.SignalR.IntegrationEvents.Models;

namespace Ordering.SignalR.IntegrationEvents.Events;

public record OrderCreatedIntegrationEvent(Order Order) : IntegrationEvent;