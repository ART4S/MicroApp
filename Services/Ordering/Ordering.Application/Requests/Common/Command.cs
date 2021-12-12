using MediatR;

namespace Ordering.Application.Requests.Common;

public abstract record Command<TResult> : IRequest<TResult>;
public abstract record Command : Command<Unit>;
