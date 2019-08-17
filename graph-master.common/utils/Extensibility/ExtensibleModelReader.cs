using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace graph_master.common.utilities.Extensibility
{
    public interface IExtensibleModelReader<TModel, TOptions>
        where TModel : class, IExtensibleDataObject
        where TOptions : ExtensibleModelGetOptions
    {
        Task<IReadOnlyList<TModel>> ReadExtensions(IReadOnlyList<TModel> models, TOptions options);
    }

    public static class ExtensibleModelReaderExtensions
    {
        public static async Task<IReadOnlyList<TModel>> ReadExtensions<TModel, TOptions>(
            this IEnumerable<IExtensibleModelReader<TModel, TOptions>> members,
            IReadOnlyList<TModel> models,
            TOptions options
        )
            where TModel : class, IExtensibleDataObject
            where TOptions : ExtensibleModelGetOptions
        {
            var membersList = members as IReadOnlyList<IExtensibleModelReader<TModel, TOptions>>;
            if (membersList != null)
            {
                for (int i = 0; i < membersList.Count; i++)
                    models = await membersList[i].ReadExtensions(models, options);
            }
            else
            {
                foreach (var member in members)
                    models = await member.ReadExtensions(models, options);
            }

            return models;
        }
    }

    public class CompositeExtensibleModelReader<TModel, TOptions> : IExtensibleModelReader<TModel, TOptions>
        where TModel : class, IExtensibleDataObject
        where TOptions : ExtensibleModelGetOptions
    {
        private readonly Func<IEnumerable<IExtensibleModelReader<TModel, TOptions>>> _membersFactory;

        public CompositeExtensibleModelReader(Func<IEnumerable<IExtensibleModelReader<TModel, TOptions>>> membersFactory)
        {
            _membersFactory = membersFactory ?? throw new System.ArgumentNullException(nameof(membersFactory));
        }

        public async Task<IReadOnlyList<TModel>> ReadExtensions(IReadOnlyList<TModel> models, TOptions options)
        {
            return await _membersFactory().ReadExtensions(models, options);
        }
    }
}