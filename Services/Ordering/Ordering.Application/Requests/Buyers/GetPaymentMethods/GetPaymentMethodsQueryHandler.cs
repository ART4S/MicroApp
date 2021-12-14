using MediatR;
using System.Data.Common;
using Dapper;
using Ordering.Application.Model.PaymentMethods;

namespace Ordering.Application.Requests.Buyers.GetPaymentMethods;

public class GetPaymentMethodsQueryHandler : IRequestHandler<GetPaymentMethodsQuery, IEnumerable<PaymentMethodInfoDto>>
{
    private readonly DbConnection _connection;

    public GetPaymentMethodsQueryHandler(DbConnection connection)
    {
        _connection = connection;
    }

    public Task<IEnumerable<PaymentMethodInfoDto>> Handle(GetPaymentMethodsQuery request, CancellationToken cancellationToken)
    {
         return _connection.QueryAsync<PaymentMethodInfoDto>(
            sql: @"
                SELECT Id, Alias, CardTypeId 
                FROM dbo.PaymentMethods
                WHERE BuyerId = @BuyerId",
            param: request);
    }
}