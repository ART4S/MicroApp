using AutoMapper;
using IntegrationServices;
using MediatR;
using Ordering.Application.Exceptions;
using Ordering.Application.IntegrationEvents.Events;
using Ordering.Application.IntegrationEvents.Models;
using Ordering.Application.Services;
using Ordering.Domian.Dictionaries;
using Ordering.Domian.Entities;

namespace Ordering.Application.Requests.Orders.CancelOrder;

public class CancelOrderCommandHandler : IRequestHandler<CancelOrderCommand>
{
    private readonly IMapper _mapper;
    private readonly IOrderingDbContext _orderingDb;
    private readonly IIntegrationEventService _integrationEvents;

    public CancelOrderCommandHandler(
        IMapper mapper,
        IOrderingDbContext orderingDb, 
        IIntegrationEventService integrationEvents)
    {
        _mapper = mapper;
        _orderingDb = orderingDb;
        _integrationEvents = integrationEvents;
    }

    private static readonly int[] CancellableStatuses = new[]
    {
        OrderStatusDict.Submitted.Id,
        OrderStatusDict.ConfirmedByUser.Id,
        OrderStatusDict.Accepted.Id,
        OrderStatusDict.Paid.Id,
    };

    public async Task<Unit> Handle(CancelOrderCommand request, CancellationToken cancellationToken)
    {
        Order order = await _orderingDb.Orders.FindAsync(request.OrderId) ??
            throw new EntityNotFoundException(nameof(Order));

        if (!CancellableStatuses.Contains(order.OrderStatusId)) return Unit.Value;

        if (order.OrderStatusId == OrderStatusDict.Paid.Id)
        {
            // TODO: return money
        }

        order.OrderStatusId = OrderStatusDict.Cancelled.Id;

        await _orderingDb.SaveChangesAsync();

        await _integrationEvents.Publish(new OrderCancelledIntegrationEvent(_mapper.Map<CancelledOrder>(order)));

        return Unit.Value;
    }
}