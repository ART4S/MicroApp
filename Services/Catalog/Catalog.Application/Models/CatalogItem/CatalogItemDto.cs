namespace Catalog.Application.Dto.CatalogItem;

public record CatalogItemDto
{
    public Guid Id { get; init; }

    public string Name { get; init; }

    public string Description { get; init; }

    public decimal? Price { get; set; }
}
