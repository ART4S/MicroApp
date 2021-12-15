namespace Web.API.Settings;

public record CatalogUrls
{
    public string BasePath { get; set; }
    public string GetItemUrl { get; set; }
    public string GetItemsUrl { get; set; }
    public string GetTypesUrl { get; set; }
    public string GetBrandsUrl { get; set; }
    public string CreateItemUrl { get; set; }
    public string UpdateItemUrl { get; set; }
    public string DeleteItemUrl { get; set; }
}
