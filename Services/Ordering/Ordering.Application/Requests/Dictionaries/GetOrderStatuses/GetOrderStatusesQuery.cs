using MediatR;
using Ordering.Application.Models.Dictionaries;

namespace Ordering.Application.Requests.Dictionaries.GetOrderStatuses;

public record GetOrderStatusesQuery : IRequest<IEnumerable<OrderStatusDictDto>>;
