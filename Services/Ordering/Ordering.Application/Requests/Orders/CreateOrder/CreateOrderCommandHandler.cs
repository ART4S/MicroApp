using AutoMapper;
using IntegrationServices;
using MediatR;
using Ordering.Application.Exceptions;
using Ordering.Application.Integration.Events;
using Ordering.Application.Integration.Models;
using Ordering.Application.Model.Identity;
using Ordering.Application.Services.Common;
using Ordering.Application.Services.DataAccess;
using Ordering.Application.Services.Identity;
using Ordering.Domian.Dictionaries;
using Ordering.Domian.Entities;

namespace Ordering.Application.Requests.Orders.CreateOrder;

public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand>
{
    private readonly IMapper _mapper;
    private readonly ICurrentTime _currentTime;
    private readonly IOrderingDbContext _orderingDb;
    private readonly IIdentityService _identityService;
    private readonly IIntegrationEventService _integrationService;

    public CreateOrderCommandHandler(
        IMapper mapper,
        ICurrentTime currentTime,
        IOrderingDbContext orderingDb,
        IIdentityService identityService,
        IIntegrationEventService integrationService)
    {
        _mapper = mapper;
        _currentTime = currentTime;
        _orderingDb = orderingDb;
        _identityService = identityService;
        _integrationService = integrationService;
    }

    public async Task<Unit> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        Buyer buyer = await CreateBuyerIfNotExists(request.BuyerId);

        Order order = new()
        {
            BuyerId = buyer.Id,
            OrderDate = _currentTime.Now,
            OrderStatusId = OrderStatusDict.Submitted.Id,
        };

        foreach (BasketItem basketItem in request.Items)
        {
            var item = _mapper.Map<OrderItem>(basketItem);
            item.IsInStock = true;
            order.OrderItems.Add(item);
        }

        await _orderingDb.Orders.AddAsync(order);

        await _orderingDb.SaveChangesAsync();

        await _integrationService.Save(new OrderCreatedIntegrationEvent(order.BuyerId, order.Id));

        return Unit.Value;
    }

    private async Task<Buyer> CreateBuyerIfNotExists(Guid buyerId)
    {
        Buyer? buyer = await _orderingDb.Buyers.FindAsync(buyerId);

        if (buyer is null)
        {
            UserDto user = await _identityService.GetUser(buyerId)
                ?? throw new InvalidRequestException($"User with id '{buyerId}' doesnt exist");

            buyer = new()
            {
                Id = user.Id,
                Name = user.Name
            };

            await _orderingDb.Buyers.AddAsync(buyer);
        }

        return buyer;
    }
}