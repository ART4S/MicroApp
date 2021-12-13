using MediatR;

namespace IdempotencyServices.Mediator;

public class IdempotentRequestHandler<TRequest, TResponse> : IRequestHandler<IdempotentRequest<TRequest, TResponse>, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IMediator _mediator;
    private readonly IClientRequestService _requestService;

    public IdempotentRequestHandler(
        IMediator mediator,
        IClientRequestService requestService)
    {
        _mediator = mediator;
        _requestService = requestService;
    }

    public async Task<TResponse?> Handle(IdempotentRequest<TRequest, TResponse> request, CancellationToken cancellationToken)
    {
        if (await _requestService.Exists(request.Id)) return default;

        await _requestService.Create(request.Id, typeof(TRequest).Name);

        return await _mediator.Send(request.OriginalRequest);
    }
}
