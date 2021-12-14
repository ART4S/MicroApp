using Ordering.Application.Requests.Abstractions;

namespace Ordering.Application.Requests.Orders.SetOrderStatusToPaid;

public record SetOrderStatusToPaidCommand(Guid OrderId) : Command;
