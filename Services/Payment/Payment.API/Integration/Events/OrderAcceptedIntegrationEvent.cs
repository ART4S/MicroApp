using IntegrationServices.Models;
using Ordering.Application.Integration.Models;

namespace Payment.API.Integration.Events;

public record OrderAcceptedIntegrationEvent(AcceptedOrder Order) : IntegrationEvent;
