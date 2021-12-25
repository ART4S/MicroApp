using AutoMapper;
using Catalog.API.Application.Models.CatalogType;
using Catalog.API.Models;

namespace Catalog.API.Application.MapperProfiles;

public class CatalogItemTypeProfile : Profile
{
    public CatalogItemTypeProfile()
    {
        CreateMap<CatalogType, CatalogTypeDto>(MemberList.Destination);
    }
}
