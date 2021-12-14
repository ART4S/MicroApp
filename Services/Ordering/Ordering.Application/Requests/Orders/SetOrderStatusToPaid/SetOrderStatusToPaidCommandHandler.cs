using AutoMapper;
using IntegrationServices;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Ordering.Application.Exceptions;
using Ordering.Application.Integration.Events;
using Ordering.Application.Integration.Models;
using Ordering.Application.Services.DataAccess;
using Ordering.Domian.Dictionaries;
using Ordering.Domian.Entities;

namespace Ordering.Application.Requests.Orders.SetOrderStatusToPaid;

public class SetOrderStatusToPaidCommandHandler : IRequestHandler<SetOrderStatusToPaidCommand>
{
    private readonly ILogger _logger;
    private readonly IMapper _mapper;
    private readonly IOrderingDbContext _orderingDb;
    private readonly IIntegrationEventService _integrationEvents;

    public SetOrderStatusToPaidCommandHandler(
        ILogger<SetOrderStatusToPaidCommandHandler> logger,
        IMapper mapper,
        IOrderingDbContext orderingDb,
        IIntegrationEventService integrationEvents)
    {
        _logger = logger;
        _mapper = mapper;
        _orderingDb = orderingDb;
        _integrationEvents = integrationEvents;
    }

    public async Task<Unit> Handle(SetOrderStatusToPaidCommand request, CancellationToken cancellationToken)
    {
        Order order = await _orderingDb.Orders
            .Include(x => x.OrderItems)
            .SingleOrDefaultAsync(x => x.Id == request.OrderId) ??
        throw new EntityNotFoundException(nameof(Order));

        if (order.OrderStatusId == OrderStatusDict.Accepted.Id)
        {
            order.OrderStatusId = OrderStatusDict.Paid.Id;

            await _orderingDb.SaveChangesAsync();

            PaidOrder paidOrder = _mapper.Map<PaidOrder>(order);

            await _integrationEvents.Save(new OrderPaidIntegrationEvent(paidOrder));
        }
        else
        {
            // TODO: log
        }

        return Unit.Value;
    }
}