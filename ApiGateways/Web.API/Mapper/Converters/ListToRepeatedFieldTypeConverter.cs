using AutoMapper;
using Google.Protobuf.Collections;

namespace Web.API.Mapper.Converters;

public class ListToRepeatedFieldTypeConverter<TListElem, TRepeatedFieldElem>
    : ITypeConverter<List<TListElem>, RepeatedField<TRepeatedFieldElem>>
{
    public RepeatedField<TRepeatedFieldElem> Convert(List<TListElem> source, RepeatedField<TRepeatedFieldElem> destination, ResolutionContext context)
    {
        destination ??= new();

        if (source is null) return destination;

        source.ForEach(element => destination.Add(context.Mapper.Map<TRepeatedFieldElem>(element)));

        return destination;
    }
}
