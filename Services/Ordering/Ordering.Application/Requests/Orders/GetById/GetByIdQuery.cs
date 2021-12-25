using MediatR;
using Ordering.Application.Models.Orders;

namespace Ordering.Application.Requests.Orders.GetById;

public record GetByIdQuery(Guid Id) : IRequest<OrderInfoDto>;
