using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.API.Attributes;
using Web.API.Models.Identity;
using Web.API.Models.Orders;
using Web.API.Models.PaymentMethods;
using Web.API.Services.Identity;
using Web.API.Services.Ordering;

namespace Web.API.Controllers;

[Authorize]
[Route("/api/ordering")]
[ApiController]
public class OrderingController : ControllerBase
{
    private readonly IIdentityService _identityService;
    private readonly IOrderingService _orderingService;

    public OrderingController(
        IOrderingService orderingService, 
        IIdentityService identityService)
    {
        _orderingService = orderingService;
        _identityService = identityService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<OrderSummaryDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetOrders()
    {
        var claims = HttpContext.User.Claims.ToList();
        var user1 = HttpContext.User;
        var user = await _identityService.GetCurrentUser();

        return Ok(await _orderingService.GetUserOrders(user.Id));
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
        UserDto user = await _identityService.GetCurrentUser();

        return Ok(await _orderingService.GetUserPaymentMethods(user.Id));
    }

    [HttpPost("paymentMethods")]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    public async Task<IActionResult> CreatePaymentMethod(PaymentMethodEditDto paymentMethod)
    {
        UserDto user = await _identityService.GetCurrentUser();

        return Ok(await _orderingService.CreateUserPaymentMethod(user.Id, paymentMethod));
    }
}