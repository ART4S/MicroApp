using MediatR;
using Ordering.Application.Models.PaymentMethods;

namespace Ordering.Application.Requests.Buyers.GetPaymentMethods;

public record GetPaymentMethodsQuery(Guid BuyerId) : IRequest<IEnumerable<PaymentMethodInfoDto>>;
