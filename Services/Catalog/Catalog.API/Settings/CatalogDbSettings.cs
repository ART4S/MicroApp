namespace Catalog.API.Settings;

public record CatalogDbSettings
{
    public string ConnectionString { get; set; }
    public string DatabaseName { get; set; }
}
