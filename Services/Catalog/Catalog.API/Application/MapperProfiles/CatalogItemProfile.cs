using AutoMapper;
using Catalog.API.Application.Models.CatalogItem;
using Catalog.API.Models;

namespace Catalog.API.Application.MapperProfiles;

public class CatalogItemProfile : Profile
{
    public CatalogItemProfile()
    {
        CreateMap<CatalogItem, CatalogItemDto>(MemberList.Destination);
        CreateMap<CatalogItem, CatalogItemInfoDto>(MemberList.Destination)
            .ForMember(dest => dest.TypeName, opt => opt.MapFrom(src => src.Type.Type))
            .ForMember(dest => dest.BrandName, opt => opt.MapFrom(src => src.Brand.Brand));
        CreateMap<CatalogItemEditDto, CatalogItem>(MemberList.Source);
    }
}
