using AutoMapper;
using Catalog.Application.Dto.CatalogBrand;
using Catalog.Domian.Entities;

namespace Catalog.Application.MapperProfiles;

public class CatalogItemBrandProfile : Profile
{
    public CatalogItemBrandProfile()
    {
        CreateMap<CatalogBrand, CatalogBrandDto>(MemberList.Destination);
    }
}

