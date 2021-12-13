using Ordering.Application.Model.Orders;
using Ordering.Application.Requests.Common;

namespace Ordering.Application.Requests.Orders.ConfirmOrder;

public record ConfirmOrderCommand(Guid orderId, OrderEditDto order) : Command;
