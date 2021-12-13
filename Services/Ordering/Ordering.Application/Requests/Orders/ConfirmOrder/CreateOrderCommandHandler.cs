using AutoMapper;
using IntegrationServices;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Ordering.Application.Integration.Events;
using Ordering.Application.Integration.Models;
using Ordering.Application.Services.DataAccess;
using Ordering.Domian.Aggregates.OrderAggregate;
using Ordering.Domian.Dictionaries;

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
            .SingleAsync(x => x.Id == request.orderId);

        _mapper.Map(request.order, order);

        order.OrderStatusId = OrderStatusDict.Confirmed.Id;

        await _orderingDb.SaveChangesAsync();

        await _integrationEvents.Save(new OrderConfirmedIntegrationEvent(Order: _mapper.Map<ConfirmedOrder>(order)));

        return Unit.Value;
    }
}