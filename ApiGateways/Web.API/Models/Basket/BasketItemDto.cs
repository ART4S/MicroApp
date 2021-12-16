namespace Web.API.Models.Basket;

public class BasketItemDto
{
	public Guid ProductId { get; set; }
	public string ProductName { get; set; }
	public decimal? UnitPrice { get; set; }
	public decimal? OldUnitPrice { get; set; }
	public int Quantity { get; set; }
}
