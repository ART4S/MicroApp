using Microsoft.AspNetCore.Mvc;
using Ordering.API.Infrastructure.Attributes;
using Ordering.Application.Model.PaymentMethods;
using Ordering.Application.Requests.Buyers.CreatePaymentMethod;
using Ordering.Application.Requests.Buyers.GetPaymentMethods;

namespace Ordering.API.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class BuyersController : BaseController
{
    [HttpGet("{buyerId:guid}/paymentMethods")]
    [ProducesResponseType(typeof(IEnumerable<PaymentMethodInfoDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPaymentMethods([RequiredNonDefault] Guid buyerId)
    {
        return Ok(await Mediator.Send(new GetPaymentMethodsQuery(buyerId)));
    }

    [HttpPost("{buyerId:guid}/paymentMethods")]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    public async Task<IActionResult> CreatePaymentMethod(
        [RequiredNonDefault] Guid buyerId, PaymentMethodEditDto paymentMethod)
    {
        return Ok(await Mediator.Send(new CreatePaymentMethodCommand(buyerId, paymentMethod)));
    }
}