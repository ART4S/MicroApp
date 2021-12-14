using MediatR;
using Ordering.Application.Exceptions;
using Ordering.Application.Services.DataAccess;
using Ordering.Domian.Dictionaries;
using Ordering.Domian.Entities;

namespace Ordering.Application.Requests.Orders.CancelOrder;

public class CancelOrderCommandHandler : IRequestHandler<CancelOrderCommand>
{
    private static readonly int[] _cancellableStatuses = new[]
{
        OrderStatusDict.Submitted.Id,
        OrderStatusDict.ConfirmedByUser.Id,
        OrderStatusDict.Accepted.Id,
        OrderStatusDict.Paid.Id,
    };

    private readonly IOrderingDbContext _orderingDb;

    public CancelOrderCommandHandler(IOrderingDbContext orderingDb)
    {
        _orderingDb = orderingDb;
    }

    public async Task<Unit> Handle(CancelOrderCommand request, CancellationToken cancellationToken)
    {
        Order order = await _orderingDb.Orders.FindAsync(request.OrderId) ??
            throw new EntityNotFoundException(nameof(Order));

        if (_cancellableStatuses.Contains(order.OrderStatusId))
        {
            if (order.OrderStatusId == OrderStatusDict.Paid.Id)
            {
                // TODO: event to payment service: return money
            }

            order.OrderStatusId = OrderStatusDict.Cancelled.Id;

            await _orderingDb.SaveChangesAsync();
        }

        return Unit.Value;
    }
}