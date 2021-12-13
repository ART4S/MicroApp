using MediatR;
using Ordering.Application.Model.Dictionaries;

namespace Ordering.Application.Requests.Dictionaries.GetCardTypes;

public record GetCardTypesQuery : IRequest<IEnumerable<CardTypeDictDto>>;
