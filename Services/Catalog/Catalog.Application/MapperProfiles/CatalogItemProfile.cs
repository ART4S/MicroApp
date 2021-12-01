using AutoMapper;
using Catalog.Application.Dto.CatalogItem;
using Catalog.Domian.Entities;

namespace Catalog.Application.MapperProfiles
{
    public class CatalogItemProfile : Profile
    {
        public CatalogItemProfile()
        {
            CreateMap<CatalogItem, CatalogItemDto>(MemberList.Destination);
            CreateMap<CatalogItem, CatalogItemInfoDto>(MemberList.Destination)
                .ForMember(dest => dest.TypeName, opt => opt.MapFrom(src => src.Type.Type))
                .ForMember(dest => dest.BrandName, opt => opt.MapFrom(src => src.Brand.Brand));
            CreateMap<CatalogItemCreateDto, CatalogItem>(MemberList.Source);
        }
    }
}
