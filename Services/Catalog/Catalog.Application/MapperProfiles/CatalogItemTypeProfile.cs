using AutoMapper;
using Catalog.Application.Dto.CatalogType;
using Catalog.Domian.Entities;

namespace Catalog.Application.MapperProfiles;

public class CatalogItemTypeProfile : Profile
{
    public CatalogItemTypeProfile()
    {
        CreateMap<CatalogType, CatalogTypeDto>(MemberList.Destination);
    }
}
