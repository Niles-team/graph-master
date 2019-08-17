using System.Runtime.Serialization;
using System.Xml;

namespace graph_master.common.utilities
{
    public static class DataContractSerializerCache<T>
    {
        public static readonly DataContractSerializer Serializer = CreateSerializer();

        private static DataContractSerializer CreateSerializer()
        {
            var xmlDictionary = new XmlDictionary();
            return new DataContractSerializer(typeof(T), new DataContractSerializerSettings
            {
                RootName = xmlDictionary.Add("root"),
                RootNamespace = xmlDictionary.Add("")
            });
        }
    }
}