using MediatR;
using Ordering.Application.Model.Dictionaries;

namespace Ordering.Application.Requests.Dictionaries.GetOrderStatuses;

public record GetOrderStatusesQuery : IRequest<IEnumerable<OrderStatusDictDto>>;
