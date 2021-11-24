using Catalog.Domian.Common;

namespace Catalog.Domian.Entities;

public class CatalogItem : BaseEntity
{
    public string Name { get; set; }

    public string Description { get; set; }

    public decimal? Price { get; set; }

    public string PictureId { get; set; }

    public Guid BrandId { get; set; }
    public CatalogItemBrand Brand { get; set; }

    public Guid TypeId { get; set; }
    public CatalogItemType Type { get; set; }
}
