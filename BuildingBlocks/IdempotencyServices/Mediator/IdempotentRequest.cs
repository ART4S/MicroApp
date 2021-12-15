using MediatR;

namespace IdempotencyServices.Mediator;

public class IdempotentRequest<TRequest, TResponse> : IRequest<TResponse>
    where TRequest : IRequest<TResponse>
{
    public IdempotentRequest(Guid id, TRequest originalRequest)
    {
        Id = id;
        OriginalRequest = originalRequest;
    }

    public Guid Id { get; }
    public TRequest OriginalRequest { get; }
}
