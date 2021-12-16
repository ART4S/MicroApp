using AutoMapper;
using GrpcBasket;
using Web.API.Models.Basket;
using Web.API.Models.Identity;
using Web.API.Services.Basket;
using Web.API.Services.Identity;
using GrpcClient = GrpcBasket.Basket.BasketClient;

namespace Web.API.Services;

public class BasketService : IBasketService
{
    private readonly IMapper _mapper;
    private readonly GrpcClient _basketClient;
    private readonly IIdentityService _identityService;

    public BasketService(
        IMapper mapper, 
        GrpcClient basketClient, 
        IIdentityService identityService)
    {
        _mapper = mapper;
        _basketClient = basketClient;
        _identityService = identityService;
    }

    public async Task<BasketDto> GetBasket()
    {
        UserDto user = await _identityService.GetCurrentUser();

        BasketReply response = await _basketClient.GetBasketAsync(new() { BuyerId = user.Id.ToString() });

        return _mapper.Map<BasketDto>(response);
    }

    public async Task<BasketDto> UpdateBasket(BasketDto basket)
    {
        UpdateBasketRequest request = _mapper.Map<UpdateBasketRequest>(basket);

        UserDto user = await _identityService.GetCurrentUser();

        request.BuyerId = user.Id.ToString();

        BasketReply response = await _basketClient.UpdateBasketAsync(request);

        return _mapper.Map<BasketDto>(response);
    }

    public async Task CheckoutBasket(Guid requestId)
    {
        UserDto user = await _identityService.GetCurrentUser();

        CheckoutBasketRequest request = new()
        {
            BuyerId = user.Id.ToString(),
            RequestId = requestId.ToString(),
        };

        await _basketClient.CheckoutBasketAsync(request);
    }
}
