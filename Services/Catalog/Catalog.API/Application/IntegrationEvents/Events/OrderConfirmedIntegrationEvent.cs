using Catalog.API.Application.IntegrationEvents.Models;
using IntegrationServices.Models;

namespace Catalog.API.Application.IntegrationEvents.Events;

public record OrderConfirmedIntegrationEvent(ConfirmedOrder Order) : IntegrationEvent;