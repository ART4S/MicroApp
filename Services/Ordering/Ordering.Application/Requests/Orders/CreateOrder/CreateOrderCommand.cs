using Ordering.Application.IntegrationEvents.Models;
using Ordering.Application.Requests.Abstractions;

namespace Ordering.Application.Requests.Orders.CreateOrder;

public record CreateOrderCommand : Command
{
    public CreateOrderCommand(Guid buyerId, string buyerName, CustomerBasket basket)
    {
        BuyerId = buyerId;
        BuyerName = buyerName;
        Basket = basket;
    }

    public Guid BuyerId { get; private init; }

    public string BuyerName { get; private init; }

    public CustomerBasket Basket { get; private init; }
}
