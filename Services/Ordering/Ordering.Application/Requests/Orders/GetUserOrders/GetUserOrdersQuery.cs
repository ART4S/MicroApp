using MediatR;
using Ordering.Application.Model.Orders;

namespace Ordering.Application.Requests.Orders.GetUserOrders;

public record class GetUserOrdersQuery(Guid UserId) : IRequest<IEnumerable<OrderSummaryDto>>;