using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace graph_master.common.utilities.Extensibility
{
    public interface IExtensibleModelValidator<TModel> where TModel : class, IExtensibleDataObject
    {
        Task AssertExtensionsState(IReadOnlyList<TModel> models);
    }

    public static class ExtensibleModelValidatorExtensions
    {
        public static async Task AssertExtensionsState<TModel>(
            this IEnumerable<IExtensibleModelValidator<TModel>> members,
            IReadOnlyList<TModel> models
        )
            where TModel : class, IExtensibleDataObject
        {
            var membersList = members as IReadOnlyList<IExtensibleModelValidator<TModel>>;
            if (membersList != null)
            {
                for (int i = 0; i < membersList.Count; i++)
                    await membersList[i].AssertExtensionsState(models);
            }
            else
            {
                foreach (var member in members)
                    await member.AssertExtensionsState(models);
            }
        }

    }

    public class CompositeExtensibleModelValidator<TModel> : IExtensibleModelValidator<TModel>
        where TModel : class, IExtensibleDataObject
    {
        private readonly Func<IEnumerable<IExtensibleModelValidator<TModel>>> _membersFactory;

        public CompositeExtensibleModelValidator(Func<IEnumerable<IExtensibleModelValidator<TModel>>> membersFactory)
        {
            _membersFactory = membersFactory ?? throw new ArgumentNullException(nameof(membersFactory));
        }

        public async Task AssertExtensionsState(IReadOnlyList<TModel> models)
        {
            await _membersFactory().AssertExtensionsState(models);
        }
    }
}