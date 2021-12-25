using Ordering.Application.IntegrationEvents.Models;
using Ordering.Application.Requests.Abstractions;

namespace Ordering.Application.Requests.Orders.CreateOrder;

public record CreateOrderCommand : Command
{
    public CreateOrderCommand(Guid buyerId, List<BasketItem> items)
    {
        BuyerId = buyerId;
        Items = items.AsReadOnly();
    }

    public Guid BuyerId { get; private init; }

    public IReadOnlyCollection<BasketItem> Items { get; private init; }
}
