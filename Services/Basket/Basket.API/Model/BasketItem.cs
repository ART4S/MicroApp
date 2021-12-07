namespace Basket.API.Model;

public class BasketItem
{
    public string Id { get; set; }
    public int CatalogItemId { get; set; }
    public string CatalogItemName { get; set; }
    public decimal? Price { get; set; }
    public decimal? OldPrice { get; set; }
    public int Quantity { get; set; }
    public string PictureUrl { get; set; }
}
