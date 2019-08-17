using System.Runtime.Serialization.Json;

namespace graph_master.common.utilities
{
    public static class DataContractJsonSerializerCache<T>
    {
        public static readonly DataContractJsonSerializer Serializer = new DataContractJsonSerializer(typeof(T));
    }
}