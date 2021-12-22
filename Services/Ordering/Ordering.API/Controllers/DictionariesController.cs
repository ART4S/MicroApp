using Microsoft.AspNetCore.Mvc;
using Ordering.Application.Model.Dictionaries;
using Ordering.Application.Requests.Dictionaries.GetCardTypes;
using Ordering.Application.Requests.Dictionaries.GetOrderStatuses;

namespace Ordering.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DictionariesController : BaseController
{
    [HttpGet("orderStatuses")]
    [ProducesResponseType(typeof(IEnumerable<OrderStatusDictDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> OrderStatuses()
    {
        return Ok(await Mediator.Send(new GetOrderStatusesQuery()));
    }

    [HttpGet("cardTypes")]
    [ProducesResponseType(typeof(IEnumerable<CardTypeDictDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> CardTypes()
    {
        return Ok(await Mediator.Send(new GetCardTypesQuery()));
    }
}