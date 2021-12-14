using Ordering.Application.Requests.Abstractions;

namespace Ordering.Application.Requests.Orders.AcceptOrder;

public record AcceptOrderCommand(Guid OrderId) : Command;
