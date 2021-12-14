using MediatR;
using Ordering.Application.Model.PaymentMethods;

namespace Ordering.Application.Requests.Buyers.GetPaymentMethods;

public record GetPaymentMethodsQuery(Guid BuyerId) : IRequest<IEnumerable<PaymentMethodInfoDto>>;
