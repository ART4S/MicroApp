namespace Web.API.Models.Catalog.CatalogItems;

public record CatalogItemInfoDto
{
    public Guid Id { get; init; }

    public string Name { get; init; }

    public string Description { get; init; }

    public string? PictureName { get; init; }

    public decimal? Price { get; init; }

    public Guid BrandId { get; init; }
    public string BrandName { get; init; }

    public Guid TypeId { get; init; }
    public string TypeName { get; init; }
}