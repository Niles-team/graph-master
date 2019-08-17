using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace graph_master.common.utilities.Extensibility
{
    public interface IExtensibleModelCreator<TModel> : IExtensibleModelValidator<TModel> where TModel : class, IExtensibleDataObject
    {
        Task<IReadOnlyList<TModel>> CreateExtensions(IReadOnlyList<TModel> models);
    }

    public static class ExtensibleModelCreatorExtensions
    {
        public static async Task<IReadOnlyList<TModel>> CreateExtensions<TModel>(
            this IEnumerable<IExtensibleModelCreator<TModel>> members,
            IReadOnlyList<TModel> models
        )
            where TModel : class, IExtensibleDataObject
        {
            var membersList = members as IReadOnlyList<IExtensibleModelCreator<TModel>>;
            if (membersList != null)
            {
                for (int i = 0; i < membersList.Count; i++)
                    models = await membersList[i].CreateExtensions(models);
            }
            else
            {
                foreach (var member in members)
                    models = await member.CreateExtensions(models);
            }

            return models;
        }
    }

    public class CompositeExtensibleModelCreator<TModel> : IExtensibleModelCreator<TModel>
        where TModel : class, IExtensibleDataObject
    {
        private readonly Func<IEnumerable<IExtensibleModelCreator<TModel>>> _membersFactory;

        public CompositeExtensibleModelCreator(Func<IEnumerable<IExtensibleModelCreator<TModel>>> membersFactory)
        {
            _membersFactory = membersFactory ?? throw new ArgumentNullException(nameof(membersFactory));
        }

        public async Task AssertExtensionsState(IReadOnlyList<TModel> models)
        {
            await _membersFactory().AssertExtensionsState(models);
        }

        public async Task<IReadOnlyList<TModel>> CreateExtensions(IReadOnlyList<TModel> models)
        {
            return await _membersFactory().CreateExtensions(models);
        }
    }
}