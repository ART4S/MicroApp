using AutoMapper;
using Ordering.Application.Model.PaymentMethods;
using Ordering.Domian.Entities;

namespace Ordering.Application.MapperProfiles;

public class PaymentMethodsProfile : Profile
{
    public PaymentMethodsProfile()
    {
        CreateMap<PaymentMethodEditDto, PaymentMethod>(MemberList.Source);
        CreateMap<PaymentMethod, PaymentMethodInfoDto>(MemberList.Destination);
    }
}
