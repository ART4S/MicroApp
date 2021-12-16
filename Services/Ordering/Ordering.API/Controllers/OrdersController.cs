using IdempotencyServices.Mediator;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Ordering.API.Infrastructure.Attributes;
using Ordering.Application.Model.Orders;
using Ordering.Application.Requests.Orders.ConfirmOrder;
using Ordering.Application.Requests.Orders.GetById;
using Ordering.Application.Requests.Orders.GetUserOrders;
using Ordering.Application.Services.Identity;

namespace Ordering.API.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class OrdersController : BaseController
{
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<OrderSummaryDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetOrders([FromServices]IIdentityService identityService)
    {
        var user = await identityService.GetCurrentUser();

        return Ok(await Mediator.Send(new GetUserOrdersQuery(user.Id)));
    }

    [HttpGet("{orderId:guid}")]
    [ProducesResponseType(typeof(OrderInfoDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetById([RequiredNonDefault]Guid orderId)
    {
        return Ok(await Mediator.Send(new GetByIdQuery(orderId)));
    }

    [HttpPut("{orderId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> ConfirmOrder(
        [FromHeader(Name = "x-requestId")][RequiredNonDefault] Guid requestId, 
        [RequiredNonDefault] Guid orderId, 
        OrderEditDto order)
    {
        await Mediator.Send(new IdempotentRequest<ConfirmOrderCommand, Unit>
        (
            id: requestId,
            originalRequest: new(orderId, order)
        ));

        return Ok();
    }
}