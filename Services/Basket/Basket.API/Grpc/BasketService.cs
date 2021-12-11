using AutoMapper;
using Basket.API.Infrastructure.DataAccess;
using Basket.API.Infrastructure.Integration.Events;
using Basket.API.Model;
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
        BasketReply response = new() { BuyerId = request.BuyerId };

        BasketEntry? entry = await _basketRepo.Get(request.BuyerId);

        if (entry is null)
            return response;

        return _mapper.Map(entry.Basket, response);
    }

    public override async Task<BasketReply> UpdateBasket(UpdateBasketRequest request, ServerCallContext context)
    {
        CustomerBasket basket = _mapper.Map<CustomerBasket>(request);

        await _basketRepo.Update(new BasketEntry(basket));

        return _mapper.Map<BasketReply>(basket);
    }

    public override async Task<Empty> CheckoutBasket(CheckoutBasketRequest request, ServerCallContext context)
    {
        BasketEntry? entry = await _basketRepo.Get(request.BuyerId);

        if (entry is null)
        {
            context.Status = new Status(StatusCode.NotFound, $"Basket for buyer {request.BuyerId} is not created yet");
            return new Empty();
        }

        _eventBus.Publish(new BasketCheckoutIntegrationEvent
        (
            requestId: Guid.Parse(request.RequestId),
            basket: entry.Basket
        ));

        return new Empty();
    }
}
