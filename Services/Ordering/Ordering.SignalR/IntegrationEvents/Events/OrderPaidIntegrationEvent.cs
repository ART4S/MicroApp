using IntegrationServices.Models;
using Ordering.Application.IntegrationEvents.Models;

namespace Ordering.SignalR.IntegrationEvents.Events;

public record OrderPaidIntegrationEvent(Order Order) : IntegrationEvent;