using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.API.Attributes;
using Web.API.Models.Orders;
using Web.API.Models.PaymentMethods;
using Web.API.Services.Ordering;

namespace Web.API.Controllers;

[Authorize]
[Route("/api/ordering")]
[ApiController]
public class OrderingController : ControllerBase
{
    private readonly IOrderingService _orderingService;

    public OrderingController(IOrderingService orderingService)
    {
        _orderingService = orderingService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<OrderSummaryDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetOrders()
    {
        return Ok(await _orderingService.GetOrders());
    }

    [HttpGet("{orderId:guid}")]
    [ProducesResponseType(typeof(OrderInfoDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetById([RequiredNonDefault] Guid orderId)
    {
        return Ok(await _orderingService.GetById(orderId));
    }

    [HttpPut("{orderId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> ConfirmOrder(
        [FromHeader(Name = "x-requestId")][RequiredNonDefault] Guid requestId,
        [RequiredNonDefault] Guid orderId,
        OrderEditDto order)
    {
        await _orderingService.ConfirmOrder(requestId, orderId, order);

        return NoContent();
    }

    [HttpGet("paymentMethods")]
    [ProducesResponseType(typeof(IEnumerable<PaymentMethodInfoDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPaymentMethods()
    {
        return Ok(await _orderingService.GetPaymentMethods());
    }

    [HttpPost("paymentMethods")]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    public async Task<IActionResult> CreatePaymentMethod(PaymentMethodEditDto paymentMethod)
    {
        return Ok(await _orderingService.CreatePaymentMethod(paymentMethod));
    }
}