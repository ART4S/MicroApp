using Catalog.Application.Integration.Models;
using IntegrationServices.Models;

namespace Catalog.Application.Integration.Events;

public record OrderConfirmedIntegrationEvent(ConfirmedOrder Order) : IntegrationEvent;