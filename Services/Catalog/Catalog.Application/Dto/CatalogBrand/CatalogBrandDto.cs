namespace Catalog.Application.Dto.CatalogBrand;

public record CatalogBrandDto
{
    public Guid Id { get; init; }

    public string Brand { get; init; }
}

