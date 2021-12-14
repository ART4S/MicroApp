using AutoMapper;
using Ordering.Application.Integration.Models;
using Ordering.Application.Model.Orders;
using Ordering.Domian.Entities;

namespace Ordering.Application.MapperProfiles;

public class OrdersProfile : Profile
{
    public OrdersProfile()
    {
        CreateMap<BasketItem, OrderItem>(MemberList.Destination)
            .ForMember(dest => dest.IsInStock, opt => opt.Ignore());

        CreateMap<Order, OrderEditDto>()
            .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.Address.Country))
            .ForMember(dest => dest.Street, opt => opt.MapFrom(src => src.Address.Street))
            .ForMember(dest => dest.State, opt => opt.MapFrom(src => src.Address.State))
            .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.Address.City))
            .ForMember(dest => dest.ZipCode, opt => opt.MapFrom(src => src.Address.ZipCode))
            .ReverseMap();

        CreateMap<Order, ConfirmedOrder>(MemberList.Destination)
            .ForMember(dest => dest.OrderId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Items, opt => opt.MapFrom(x => x.OrderItems));

        CreateMap<OrderItem, ConfirmedOrderItem>(MemberList.Destination);

        CreateMap<Order, PaidOrder>(MemberList.Destination)
            .ForMember(dest => dest.OrderId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Items, opt => opt.MapFrom(x => x.OrderItems));

        CreateMap<OrderItem, PaidOrderItem>(MemberList.Destination);
    }
}