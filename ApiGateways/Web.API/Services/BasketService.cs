using AutoMapper;
using GrpcBasket;
using Web.API.Models.Basket;
using Web.API.Models.Identity;
using GrpcClient = GrpcBasket.Basket.BasketClient;

namespace Web.API.Services;

public class BasketService : IBasketService
{
    private readonly IMapper _mapper;
    private readonly GrpcClient _basketClient;
    private readonly IUserService _userService;

    public BasketService(
        IMapper mapper,
        GrpcClient basketClient,
        IUserService userService)
    {
        _mapper = mapper;
        _basketClient = basketClient;
        _userService = userService;
    }

    public async Task<BasketDto> GetBasket()
    {
        User user = await _userService.GetCurrentUser();

        BasketReply response = await _basketClient.GetBasketAsync(new() { BuyerId = user.Id.ToString() });

        return _mapper.Map<BasketDto>(response);
    }

    public async Task<BasketDto> UpdateBasket(BasketDto basket)
    {
        UpdateBasketRequest request = _mapper.Map<UpdateBasketRequest>(basket);

        User user = await _userService.GetCurrentUser();

        request.BuyerId = user.Id.ToString();

        BasketReply response = await _basketClient.UpdateBasketAsync(request);

        return _mapper.Map<BasketDto>(response);
    }

    public async Task CheckoutBasket(Guid requestId)
    {
        User user = await _userService.GetCurrentUser();

        CheckoutBasketRequest request = new()
        {
            BuyerId = user.Id.ToString(),
            RequestId = requestId.ToString(),
        };

        await _basketClient.CheckoutBasketAsync(request);
    }
}
