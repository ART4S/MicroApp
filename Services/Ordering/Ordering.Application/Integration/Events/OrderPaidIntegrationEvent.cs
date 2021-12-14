using IntegrationServices.Models;
using Ordering.Application.Integration.Models;

namespace Ordering.Application.Integration.Events;

public record OrderPaidIntegrationEvent(PaidOrder Order) : IntegrationEvent;