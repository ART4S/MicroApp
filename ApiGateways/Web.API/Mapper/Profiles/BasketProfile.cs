using AutoMapper;
using GrpcBasket;
using Web.API.Models.Basket;

namespace Basket.API.Infrastructure.Mapper.Profiles;

public class BasketProfile : Profile
{
    public BasketProfile()
    {
        CreateMap<string, Guid>().ConvertUsing(x => new Guid(x));
        CreateMap<Guid, string>().ConvertUsing(x => x.ToString());

        CreateMap<BasketItemReply, BasketItemDto>().ReverseMap();
        CreateMap<BasketDto, UpdateBasketRequest>(MemberList.Source);
        CreateMap<BasketReply, BasketDto>(MemberList.Destination);
    }
}
