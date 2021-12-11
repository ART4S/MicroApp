using AutoMapper;
using IntegrationServices;
using MediatR;
using Ordering.Application.Integration.Events;
using Ordering.Application.Integration.Models;
using Ordering.Application.Services.Common;
using Ordering.Application.Services.DataAccess;
using Ordering.Domian.Aggregates.OrderAggregate;
using Ordering.Domian.Dictionaries;
using Ordering.Domian.Entities.OrderAggregate;

namespace Ordering.Application.Requests.Orders.CreateOrder;

public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand>
{
    private readonly IMapper _mapper;
    private readonly ICurrentTime _currentTime;
    private readonly IOrderingDbContext _orderingDb;
    private readonly IIntegrationEventService _integrationService;

    public CreateOrderCommandHandler(
        IMapper mapper,
        ICurrentTime currentTime,
        IOrderingDbContext orderingDb,
        IIntegrationEventService integrationService)
    {
        _mapper = mapper;
        _currentTime = currentTime;
        _orderingDb = orderingDb;
        _integrationService = integrationService;
    }

    public async Task<Unit> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        Order order = new()
        {
            BuyerId = request.BuyerId,
            OrderDate = _currentTime.Now,
            OrderStatusId = OrderStatusDict.Submitted.Id,
        };

        foreach (BasketItem item in request.Items)
            order.OrderItems.Add(_mapper.Map<OrderItem>(item));

        await _orderingDb.Orders.AddAsync(order);

        await _orderingDb.SaveChangesAsync();

        await _integrationService.Save(new OrderStartedIntegrationEvent(order.BuyerId, order.Id));

        return Unit.Value;
    }
}