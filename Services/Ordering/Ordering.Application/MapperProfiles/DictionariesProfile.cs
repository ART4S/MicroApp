using AutoMapper;
using Ordering.Application.Models.Dictionaries;
using Ordering.Domian.Dictionaries;

namespace Ordering.Application.MapperProfiles;

public class DictionariesProfile : Profile
{
    public DictionariesProfile()
    {
        CreateMap<OrderStatusDict, OrderStatusDictDto>(MemberList.Destination);
        CreateMap<CardTypeDict, CardTypeDictDto>(MemberList.Destination);
    }
}