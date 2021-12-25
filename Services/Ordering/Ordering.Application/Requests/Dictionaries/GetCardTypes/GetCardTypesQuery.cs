using MediatR;
using Ordering.Application.Models.Dictionaries;

namespace Ordering.Application.Requests.Dictionaries.GetCardTypes;

public record GetCardTypesQuery : IRequest<IEnumerable<CardTypeDictDto>>;
