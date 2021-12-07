using AutoMapper;
using Google.Protobuf.Collections;

namespace Basket.API.Infrastructure.Mapper.Converters;

public class RepeatedFieldToListTypeConverter<TRepeatedFieldElem, TListElem>
    : ITypeConverter<RepeatedField<TRepeatedFieldElem>, List<TListElem>>
{
    public List<TListElem> Convert(RepeatedField<TRepeatedFieldElem> source, List<TListElem> destination, ResolutionContext context)
    {
        destination ??= new();

        if (source is null) return destination;

        foreach (TRepeatedFieldElem item in source)
            destination.Add(context.Mapper.Map<TListElem>(item));

        return destination;
    }
}
