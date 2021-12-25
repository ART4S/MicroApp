namespace Catalog.API.Application.Models.CatalogBrand;

public record CatalogBrandDto
{
    public Guid Id { get; init; }

    public string Brand { get; init; }
}

