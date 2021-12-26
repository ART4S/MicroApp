using AutoMapper;
using Basket.API.Infrastructure.DataAccess;
using Basket.API.Infrastructure.IntegrationEvents.Events;
using Basket.API.Models;
using EventBus.Abstractions;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;

namespace GrpcBasket;

public class BasketService : Basket.BasketBase
{
    private readonly IEventBus _eventBus;
    private readonly IBasketRepository _basketRepo;
    private readonly IMapper _mapper;

    public BasketService(IEventBus eventBus, IBasketRepository basketRepo, IMapper mapper)
    {
        _eventBus = eventBus;
        _basketRepo = basketRepo;
        _mapper = mapper;
    }

    public override async Task<BasketReply> GetBasket(GetBasketRequest request, ServerCallContext context)
    {
        BasketEntry? entry = await _basketRepo.Get(request.UserId);

        return entry is null ? new() : _mapper.Map<BasketReply>(entry.Basket);
    }

    public override async Task<BasketReply> UpdateBasket(UpdateBasketRequest request, ServerCallContext context)
    {
        var basket = _mapper.Map<CustomerBasket>(request);

        await _basketRepo.Update(new BasketEntry(basket) { UserId = request.UserId });

        return _mapper.Map<BasketReply>(basket);
    }

    public override async Task<Empty> CheckoutBasket(CheckoutBasketRequest request, ServerCallContext context)
    {
        BasketEntry? entry = await _basketRepo.Get(request.UserId);

        if (entry is null)
        {
            context.Status = new Status(StatusCode.NotFound, $"Basket for user '{request.UserId}' not found");
            return new Empty();
        }

        _eventBus.Publish(new BasketCheckoutIntegrationEvent
        (
            RequestId: request.RequestId,
            UserId: request.UserId,
            UserName: request.UserName,
            Basket: entry.Basket
        ));

        return new Empty();
    }
}
