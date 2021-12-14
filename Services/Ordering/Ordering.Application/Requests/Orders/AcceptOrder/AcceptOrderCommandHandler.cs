using AutoMapper;
using IntegrationServices;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Ordering.Application.Services.DataAccess;
using Ordering.Domian.Dictionaries;
using Ordering.Domian.Entities;

namespace Ordering.Application.Requests.Orders.AcceptOrder;

public class AcceptOrderCommandHandler : IRequestHandler<AcceptOrderCommand>
{
    private readonly IMapper _mapper;
    private readonly IOrderingDbContext _orderingDb;
    private readonly IIntegrationEventService _integrationEvents;

    public AcceptOrderCommandHandler(
        IMapper mapper,
        IOrderingDbContext orderingDb,
        IIntegrationEventService integrationEvents)
    {
        _mapper = mapper;
        _orderingDb = orderingDb;
        _integrationEvents = integrationEvents;
    }

    public async Task<Unit> Handle(AcceptOrderCommand request, CancellationToken cancellationToken)
    {
        Order order = await _orderingDb.Orders.SingleAsync(x => x.Id == request.OrderId);

        if (order.OrderStatusId == OrderStatusDict.ConfirmedByUser.Id)
        {
            order.OrderStatusId = OrderStatusDict.Accepted.Id;

            await _orderingDb.SaveChangesAsync();
        }

        // TODO: order accepted event

        return Unit.Value;
    }
}