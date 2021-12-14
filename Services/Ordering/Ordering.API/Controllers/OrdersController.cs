using IdempotencyServices.Mediator;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Ordering.API.Infrastructure.Attributes;
using Ordering.Application.Model.Orders;
using Ordering.Application.Requests.Orders.ConfirmOrder;
using Ordering.Application.Requests.Orders.GetById;
using Ordering.Application.Requests.Orders.GetUserOrders;

namespace Ordering.API.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class OrdersController : BaseController
{
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<OrderSummaryDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUserOrders(
        [FromHeader(Name = "x-userId")][RequiredNonDefault] Guid userId)
    {
        return Ok(await Mediator.Send(new GetUserOrdersQuery(userId)));
    }

    [HttpGet("{orderId:guid}")]
    [ProducesResponseType(typeof(OrderInfoDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetById([RequiredNonDefault]Guid orderId)
    {
        return Ok(await Mediator.Send(new GetByIdQuery(orderId)));
    }

    [HttpPut("{orderId:guid}")]
    [ProducesResponseType(typeof(IEnumerable<OrderSummaryDto>), StatusCodes.Status200OK)]
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