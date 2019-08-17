using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace graph_master.common.utilities.Extensibility
{
    public interface IExtensibleModelUpdater<TModel> : IExtensibleModelValidator<TModel> where TModel : class, IExtensibleDataObject
    {
        Task<IReadOnlyList<TModel>> UpdateExtensions(IReadOnlyList<TModel> models);
    }

    public static class ExtensibleModelUpdaterExtensions
    {
        public static async Task<IReadOnlyList<TModel>> UpdateExtensions<TModel>(
            this IEnumerable<IExtensibleModelUpdater<TModel>> members,
            IReadOnlyList<TModel> models
        )
            where TModel : class, IExtensibleDataObject
        {
            var membersList = members as IReadOnlyList<IExtensibleModelUpdater<TModel>>;
            if (membersList != null)
            {
                for (int i = 0; i < membersList.Count; i++)
                    models = await membersList[i].UpdateExtensions(models);
            }
            else
            {
                foreach (var member in members)
                    models = await member.UpdateExtensions(models);
            }

            return models;
        }
    }

    public class CompositeExtensibleModelUpdater<TModel> : IExtensibleModelUpdater<TModel>
        where TModel : class, IExtensibleDataObject
    {
        private readonly Func<IEnumerable<IExtensibleModelUpdater<TModel>>> _membersFactory;

        public CompositeExtensibleModelUpdater(Func<IEnumerable<IExtensibleModelUpdater<TModel>>> membersFactory)
        {
            _membersFactory = membersFactory ?? throw new ArgumentNullException(nameof(membersFactory));
        }

        public async Task AssertExtensionsState(IReadOnlyList<TModel> models)
        {
            await _membersFactory().AssertExtensionsState(models);
        }

        public async Task<IReadOnlyList<TModel>> UpdateExtensions(IReadOnlyList<TModel> models)
        {
            return await _membersFactory().UpdateExtensions(models);
        }
    }
}