namespace Catalog.Application.Dto.CatalogType;

public record CatalogTypeDto
{
    public Guid Id { get; init; }

    public string Type { get; init; }
}
