using Microsoft.AspNetCore.Mvc;
using Web.API.Attributes;
using Web.API.Models.Basket;
using Web.API.Services.Basket;

namespace Web.API.Controllers;

[Route("/api/v1/basket")]
[ApiController]
public class BasketController : ControllerBase
{
    private readonly IBasketService _basketService;

    public BasketController(IBasketService basketService)
    {
        _basketService = basketService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(BasketDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetBasket()
    {
        return Ok(await _basketService.GetBasket());
    }

    [HttpPost]
    [ProducesResponseType(typeof(BasketDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateBasket(BasketDto basket)
    {
        return Ok(await _basketService.UpdateBasket(basket));
    }

    [HttpPost("checkout")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> CheckoutBasket(
        [FromHeader(Name = "x-requestId")][RequiredNonDefault] Guid requestId)
    {
        await _basketService.CheckoutBasket(requestId);

        return Ok();
    }
}