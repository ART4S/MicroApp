namespace Web.API.Models.Catalog.CatalogItems;

public record CatalogItemDto
{
    public Guid Id { get; init; }

    public string Name { get; init; }

    public string Description { get; init; }

    public decimal? Price { get; set; }
}
