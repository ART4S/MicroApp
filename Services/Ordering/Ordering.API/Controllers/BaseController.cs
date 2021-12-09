using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Ordering.API.Controllers;

public class BaseController : ControllerBase
{
    protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<IMediator>();
    private IMediator? _mediator;
}
