using Microsoft.AspNetCore.Mvc;
using Web.API.Models.Dictionaries;
using Web.API.Services.Ordering;

namespace Web.API.Controllers;

[Route("/api/v1")]
[ApiController]
public class DictionariesController : ControllerBase
{
    private readonly IOrderingService _orderingService;

    public DictionariesController(IOrderingService orderingService)
    {
        _orderingService = orderingService;
    }

    [HttpGet("ordering/dictionaries/orderStatuses")]
    [ProducesResponseType(typeof(IEnumerable<OrderStatusDictDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> OrderStatuses()
    {
        return Ok(await _orderingService.GetOrderStatusesDict());
    }

    [HttpGet("ordering/dictionaries/cardTypes")]
    [ProducesResponseType(typeof(IEnumerable<CardTypeDictDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> CardTypes()
    {
        return Ok(await _orderingService.GetCardTypesDict());
    }
}
