using MediatR;
using Microsoft.EntityFrameworkCore;
using Ordering.Application.Services.DataAccess;
using Ordering.Domian.Entities;

namespace Ordering.Application.Requests.Orders.UpdateItemsInStock;

public class UpdateItemsInStockCommandHandler : IRequestHandler<UpdateItemsInStockCommand>
{
    private readonly IOrderingDbContext _orderingDb;

    public UpdateItemsInStockCommandHandler(IOrderingDbContext orderingDb)
    {
        _orderingDb = orderingDb;
    }

    public async Task<Unit> Handle(UpdateItemsInStockCommand request, CancellationToken cancellationToken)
    {
        var items = await _orderingDb.OrderItems
            .Where(x => x.OrderId == request.OrderId)
            .ToListAsync();

        var itemsInStock = request.Items.ToDictionary(x => x.ProductId);

        foreach(OrderItem item in items)
        {
            if (itemsInStock.TryGetValue(item.Id, out var itemInStock))
                item.IsInStock = itemInStock.IsInStock;
            else
                item.IsInStock = false;
        }

        await _orderingDb.SaveChangesAsync();

        return Unit.Value;
    }
}