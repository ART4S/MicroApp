using MediatR;
using Ordering.Application.Model.Dictionaries;
using System.Data.Common;
using Dapper;

namespace Ordering.Application.Requests.Dictionaries.GetOrderStatuses;

public class GetOrderStatusesQueryHandler : IRequestHandler<GetOrderStatusesQuery, IEnumerable<OrderStatusDictDto>>
{
    private readonly DbConnection _connection;

    public GetOrderStatusesQueryHandler(DbConnection connection)
    {
        _connection = connection;
    }

    public Task<IEnumerable<OrderStatusDictDto>> Handle(GetOrderStatusesQuery request, CancellationToken cancellationToken)
    {
        return _connection.QueryAsync<OrderStatusDictDto>("SELECT * FROM dbo.OrderStatusesDict");
    }
}
