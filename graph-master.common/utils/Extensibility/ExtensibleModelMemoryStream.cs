using System.IO;
using System.Runtime.Serialization;

namespace graph_master.common.utilities.Extensibility
{
    public class ExtensibleModelMemoryStream<TModel> : MemoryStream
        where TModel : class, IExtensibleDataObject
    {
        public TExtendedModel ExtendModel<TExtendedModel>(TModel model)
            where TExtendedModel : TModel, IExtensibleDataObject
        {
            return DataContractModelConverter<TModel, TExtendedModel>.Convert(this, model);
        }

        public TModel PackModel<TExtendedModel>(TExtendedModel extendedModel)
            where TExtendedModel : TModel, IExtensibleDataObject
        {
            return DataContractModelConverter<TExtendedModel, TModel>.Convert(this, extendedModel);
        }
    }
}