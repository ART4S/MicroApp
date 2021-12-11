using FluentValidation;
using Ordering.Application.Requests.Orders.CreateOrder;

namespace Ordering.Application.Validation;

public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator()
    {
    }
}
