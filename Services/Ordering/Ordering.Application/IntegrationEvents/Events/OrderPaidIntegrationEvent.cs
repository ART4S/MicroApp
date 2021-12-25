using IntegrationServices.Models;
using Ordering.Application.IntegrationEvents.Models;

namespace Ordering.Application.IntegrationEvents.Events;

public record OrderPaidIntegrationEvent(PaidOrder Order) : IntegrationEvent;