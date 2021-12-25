using AutoMapper;
using IntegrationServices;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Ordering.Application.IntegrationEvents.Events;
using Ordering.Application.IntegrationEvents.Models;
using Ordering.Application.Services;
using Ordering.Domian.Dictionaries;
using Ordering.Domian.Entities;

namespace Ordering.Application.Requests.Orders.ConfirmOrder;

public class ConfirmOrderCommandHandler : IRequestHandler<ConfirmOrderCommand>
{
    private readonly IMapper _mapper;
    private readonly IOrderingDbContext _orderingDb;
    private readonly IIntegrationEventService _integrationEvents;

    public ConfirmOrderCommandHandler(
        IMapper mapper,
        IOrderingDbContext orderingDb,
        IIntegrationEventService integrationEvents)
    {
        _mapper = mapper;
        _orderingDb = orderingDb;
        _integrationEvents = integrationEvents;
    }

    public async Task<Unit> Handle(ConfirmOrderCommand request, CancellationToken cancellationToken)
    {
        Order order = await _orderingDb.Orders
            .Include(x => x.Address)
            .Include(x => x.OrderItems)
            .SingleAsync(x => x.Id == request.OrderId);

        _mapper.Map(request.Order, order);

        order.OrderStatusId = OrderStatusDict.ConfirmedByUser.Id;

        await _orderingDb.SaveChangesAsync();

        await _integrationEvents.Publish(new OrderConfirmedIntegrationEvent(Order: _mapper.Map<ConfirmedOrder>(order)));

        return Unit.Value;
    }
}