using AutoMapper;
using Ordering.Application.Integration.Models;
using Ordering.Domian.Entities.OrderAggregate;

namespace Ordering.Application.MapperProfiles;

public class OrderProfile : Profile
{
    public OrderProfile()
    {
        CreateMap<BasketItem, OrderItem>(MemberList.Destination);
    }
}
