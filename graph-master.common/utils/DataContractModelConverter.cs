using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

namespace graph_master.common.utilities
{
    public static class DataContractModelConverter<TSource, TTarget>
        where TSource: IExtensibleDataObject
        where TTarget: IExtensibleDataObject
    {
        public static TTarget Convert(Stream stream, TSource source)
        {
            var sourceSerializer = DataContractSerializerCache<TSource>.Serializer;
            var targetSerializer = DataContractSerializerCache<TTarget>.Serializer;
            stream.SetLength(0);
            sourceSerializer.WriteObject(stream, source);
            stream.Position = 0;
            return (TTarget)targetSerializer.ReadObject(stream);
        }
    }
}