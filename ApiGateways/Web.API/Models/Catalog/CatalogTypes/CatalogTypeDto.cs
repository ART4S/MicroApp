namespace Web.API.Models.Catalog.CatalogTypes;

public record CatalogTypeDto
{
    public Guid Id { get; init; }

    public string Type { get; init; }
}
