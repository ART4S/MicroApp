using IntegrationServices.Model;
using Ordering.Application.Integration.Models;

namespace Ordering.Application.Integration.Events;

public record OrderInStockCheckedIntegrationEvent(OrderInStock Order) : IntegrationEvent;