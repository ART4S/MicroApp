using AutoMapper;
using Catalog.API.Application.Models.CatalogBrand;
using Catalog.API.Models;

namespace Catalog.API.Application.MapperProfiles;

public class CatalogItemBrandProfile : Profile
{
    public CatalogItemBrandProfile()
    {
        CreateMap<CatalogBrand, CatalogBrandDto>(MemberList.Destination);
    }
}

