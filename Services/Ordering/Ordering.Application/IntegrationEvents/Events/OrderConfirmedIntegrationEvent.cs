using IntegrationServices.Models;
using Ordering.Application.IntegrationEvents.Models;

namespace Ordering.Application.IntegrationEvents.Events;

public record OrderConfirmedIntegrationEvent(ConfirmedOrder Order) : IntegrationEvent;