using AutoMapper;
using Basket.API.Infrastructure.DataAccess;
using Basket.API.Model;
using Grpc.Core;

namespace GrpcBasket;

public class BasketService : Basket.BasketBase
{
    private readonly IBasketRepository _basketRepo;
    private readonly IMapper _mapper;

    public BasketService(IBasketRepository basketRepo, IMapper mapper)
    {
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

        await _basketRepo.AddOrUpdate(basket);

        return _mapper.Map<BasketReply>(basket);
    }
}
