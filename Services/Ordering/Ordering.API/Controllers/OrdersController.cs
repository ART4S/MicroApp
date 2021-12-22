using IdempotencyServices.Mediator;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ordering.API.Infrastructure.Attributes;
using Ordering.API.Models;
using Ordering.API.Services;
using Ordering.Application.Model.Orders;
using Ordering.Application.Requests.Orders.ConfirmOrder;
using Ordering.Application.Requests.Orders.GetById;
using Ordering.Application.Requests.Orders.GetUserOrders;

namespace Ordering.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrdersController : BaseController
{
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<OrderSummaryDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetOrders([FromServices]IBuyerService buyerService)
    {
        BuyerInfo buyer = await buyerService.GetCurrentBuyer();

        return Ok(await Mediator.Send(new GetUserOrdersQuery(buyer.Id)));
    }

    [HttpGet("{orderId:guid}")]
    [ProducesResponseType(typeof(OrderInfoDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetById([RequiredNonDefault]Guid orderId)
    {
        return Ok(await Mediator.Send(new GetByIdQuery(orderId)));
    }

    [HttpPut("{orderId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateOrder(
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