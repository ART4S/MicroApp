namespace Catalog.API.Models;

public class CatalogItem : BaseEntity
{
    public string Name { get; set; }

    public string? Description { get; set; }

    public decimal? Price { get; set; }

    public string? PictureName { get; set; }

    public int AvailableInStock { get; set; }

    public CatalogBrand Brand { get; set; }

    public CatalogType Type { get; set; }
}
