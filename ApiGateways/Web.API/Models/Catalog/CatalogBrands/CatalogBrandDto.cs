namespace Web.API.Models.Catalog.CatalogBrands;

public record CatalogBrandDto
{
    public Guid Id { get; init; }

    public string Brand { get; init; }
}

