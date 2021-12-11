using MediatR;

namespace Catalog.Application.Requests.Abstractions;

public abstract record Command<TResult> : IRequest<TResult>;
public abstract record Command : Command<Unit>;
