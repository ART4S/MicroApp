namespace Catalog.API.Application.Models.CatalogItem;

public record CatalogItemDto
{
    public Guid Id { get; init; }

    public string Name { get; init; }

    public string Description { get; init; }

    public string? PictureName { get; set; }

    public decimal? Price { get; set; }
}
