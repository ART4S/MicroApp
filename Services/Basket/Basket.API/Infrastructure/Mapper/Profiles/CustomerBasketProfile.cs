using AutoMapper;
using Basket.API.Model;
using GrpcBasket;

namespace Basket.API.Infrastructure.Mapper.Profiles;

public class CustomerBasketProfile : Profile
{
    public CustomerBasketProfile()
    {
        CreateMap<BasketItem, BasketItemReply>().ReverseMap();
        CreateMap<CustomerBasket, BasketReply>(MemberList.Destination);
        CreateMap<UpdateBasketRequest, CustomerBasket>(MemberList.Source);
    }
}
