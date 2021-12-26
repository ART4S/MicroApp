using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ordering.API.Models;
using Ordering.API.Services;
using Ordering.Application.Models.PaymentMethods;
using Ordering.Application.Requests.Buyers.CreatePaymentMethod;
using Ordering.Application.Requests.Buyers.GetPaymentMethods;

namespace Ordering.API.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class PaymentMethodsController : BaseController
{
    private readonly IBuyerService _buyerService;

    public PaymentMethodsController(IBuyerService buyerService)
    {
        _buyerService = buyerService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<PaymentMethodInfoDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPaymentMethods()
    {
        BuyerInfo buyer = await _buyerService.GetCurrentBuyer();

        return Ok(await Mediator.Send(new GetPaymentMethodsQuery(buyer.Id)));
    }

    [HttpPost]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    public async Task<IActionResult> CreatePaymentMethod(PaymentMethodEditDto paymentMethod)
    {
        BuyerInfo buyer = await _buyerService.GetCurrentBuyer();

        return Ok(await Mediator.Send(new CreatePaymentMethodCommand(buyer.Id, paymentMethod)));
    }
}