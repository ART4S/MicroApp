namespace Web.API.Models.Catalog.CatalogItems;

public record CatalogItemEditDto
{
    public string Name { get; init; }

    public string Description { get; init; }

    public decimal? Price { get; set; }

    public Guid BrandId { get; set; }

    public Guid TypeId { get; set; }
}
