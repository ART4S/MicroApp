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

        CustomerBasket? basket = await _basketRepo.Get(request.BuyerId);

        if (basket is null)
            return response;

        return _mapper.Map(basket, response);
    }

    public override async Task<BasketReply> UpdateBasket(UpdateBasketRequest request, ServerCallContext context)
    {
        CustomerBasket basket = _mapper.Map<CustomerBasket>(request);

        await _basketRepo.Update(basket);

        return _mapper.Map<BasketReply>(basket);
    }

    public override async Task<Empty> CheckoutBasket(CheckoutBasketRequest request, ServerCallContext context)
    {
        CustomerBasket? basket = await _basketRepo.Get(request.BuyerId);

        if (basket is null)
        {
            context.Status = new Status(StatusCode.NotFound, $"Basket for buyer {request.BuyerId} is not created yet");
            return new Empty();
        }

        _eventBus.Publish(new BasketCheckoutIntegrationEvent
        (
            requestId: Guid.Parse(request.RequestId),
            city: request.City,
            street: request.Street,
            state: request.State,
            country: request.Country,
            zipCode: request.ZipCode,
            cardNumber: request.CardNumber,
            cardHolderName: request.CardHolderName,
            cardExpiration: request.CardExpiration.ToDateTime(),
            cardSecurityNumber: request.CardSecurityNumber,
            cardTypeId: Guid.Parse(request.CardTypeId),
            basket: basket
        ));

        return new Empty();
    }
}
