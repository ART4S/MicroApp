using System.Text.Json.Serialization;

namespace Catalog.Application.Integration.Model;

public class ConfirmedOrderItem
{
    [JsonInclude]
    public Guid ProductId { get; private set; }

    [JsonInclude]
    public int Quantity { get; private set; }
}
