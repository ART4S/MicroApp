using Catalog.Domian.Common;

namespace Catalog.Domian.Entities;

public class CatalogItem : BaseEntity
{
    public string Name { get; set; }

    public string? Description { get; set; }

    public decimal? Price { get; set; }

    public string? PictureName { get; set; }

    public Guid BrandId { get; set; }
    public CatalogBrand Brand { get; set; }

    public Guid TypeId { get; set; }
    public CatalogType Type { get; set; }
}
