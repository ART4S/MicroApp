using Catalog.Application.Integration.Model;
using IntegrationServices.Model;

namespace Catalog.Application.Integration.Events;

public record OrderConfirmedIntegrationEvent(ConfirmedOrder order) : IntegrationEvent;