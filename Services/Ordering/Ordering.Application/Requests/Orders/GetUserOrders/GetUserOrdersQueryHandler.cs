using MediatR;
using System.Data.Common;
using Dapper;
using Ordering.Application.Model.Orders;

namespace Ordering.Application.Requests.Orders.GetUserOrders;

public class GetUserOrdersQueryHandler : IRequestHandler<GetUserOrdersQuery, IEnumerable<OrderSummaryDto>>
{
    private readonly DbConnection _connection;

    public GetUserOrdersQueryHandler(DbConnection connection)
    {
        _connection = connection;
    }

    public Task<IEnumerable<OrderSummaryDto>> Handle(GetUserOrdersQuery request, CancellationToken cancellationToken)
    {
        return _connection.QueryAsync<OrderSummaryDto>(
            sql: @"
                SELECT 
                    o.Id, 
					o.OrderDate AS Date, 
					o.OrderStatusId AS StatusId,
					SUM(oi.Quantity * oi.UnitPrice) AS Total
                FROM dbo.Orders AS o
				LEFT JOIN dbo.OrderItems AS oi ON o.Id = oi.OrderId
				WHERE o.BuyerId = @UserId
				GROUP BY o.Id, o.OrderDate, o.OrderStatusId",
            param: request);
    }
}
