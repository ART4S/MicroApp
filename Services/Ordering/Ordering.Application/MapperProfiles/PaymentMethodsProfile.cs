using AutoMapper;
using Ordering.Application.IntegrationEvents.Models;
using Ordering.Application.Models.PaymentMethods;
using Ordering.Domian.Entities;

namespace Ordering.Application.MapperProfiles;

public class PaymentMethodsProfile : Profile
{
    public PaymentMethodsProfile()
    {
        CreateMap<PaymentMethodEditDto, PaymentMethod>(MemberList.Source);
        CreateMap<PaymentMethod, PaymentMethodInfoDto>(MemberList.Destination);
        CreateMap<PaymentMethod, BuyerCardInfo>(MemberList.Destination);
    }
}
