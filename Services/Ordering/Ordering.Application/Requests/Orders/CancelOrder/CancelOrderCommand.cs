using Ordering.Application.Requests.Abstractions;

namespace Ordering.Application.Requests.Orders.CancelOrder;

public record CancelOrderCommand(Guid OrderId) : Command;
