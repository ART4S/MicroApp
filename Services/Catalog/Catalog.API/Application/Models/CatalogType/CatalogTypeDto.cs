namespace Catalog.API.Application.Models.CatalogType;

public record CatalogTypeDto
{
    public Guid Id { get; init; }

    public string Type { get; init; }
}
