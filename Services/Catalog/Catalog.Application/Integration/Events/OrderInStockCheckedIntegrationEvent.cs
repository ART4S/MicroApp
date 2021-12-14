using Catalog.Application.Integration.Model;
using IntegrationServices.Model;

namespace Catalog.Application.Integration.Events;

public record OrderInStockCheckedIntegrationEvent(OrderInStock Order) : IntegrationEvent;