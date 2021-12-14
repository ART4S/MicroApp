using Ordering.Application.Integration.Models;
using Ordering.Application.Requests.Abstractions;

namespace Ordering.Application.Requests.Orders.UpdateItemsInStock;

public record UpdateItemsInStockCommand(Guid OrderId, List<OrderItemInStock> Items) : Command;
