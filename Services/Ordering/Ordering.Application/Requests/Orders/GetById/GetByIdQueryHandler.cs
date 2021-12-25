using MediatR;
using System.Data.Common;
using Dapper;
using Ordering.Application.Exceptions;
using Ordering.Domian.Entities;
using Ordering.Application.Models.Orders;

namespace Ordering.Application.Requests.Orders.GetById;

public class GetByIdQueryHandler : IRequestHandler<GetByIdQuery, OrderInfoDto>
{
    private readonly DbConnection _connection;

    public GetByIdQueryHandler(DbConnection connection)
    {
        _connection = connection;
    }

    public async Task<OrderInfoDto> Handle(GetByIdQuery request, CancellationToken cancellationToken)
    {
        OrderInfoDto order = (await _connection.QueryAsync<OrderInfoDto, OrderItemDto, OrderInfoDto>(
                sql: @"
                    SELECT 
                        o.Id, o.OrderDate AS Date, o.OrderStatusId AS StatusId, o.Description, o.PaymentMethodId, 
                        oa.Street, oa.City, oa.State, oa.Country, oa.ZipCode,
                        oi.UnitPrice, oi.Quantity, oi.ProductId, oi.IsInStock
                    FROM dbo.Orders AS o
                    LEFT JOIN dbo.OrderItems AS oi ON o.Id = oi.OrderId
                    LEFT JOIN dbo.OrderAddresses AS oa ON o.Id = oa.OrderId
                    WHERE o.Id = @Id",
                map: (order, item) =>
                {
                    order.OrderItems.Add(item);
                    return order;
                },
                param: request,
                splitOn: "UnitPrice"))
            .FirstOrDefault() ?? throw new EntityNotFoundException(nameof(Order));

        return order;
    }
}
