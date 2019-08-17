using System;
using System.Runtime.Serialization;
using Microsoft.Extensions.DependencyInjection;

namespace graph_master.common.utilities.Extensibility
{
    public static class ExtensibilityExtensions
    {
        public static IExtensibleModelReader<TModel, TOptions> ComposeExtensibleModelReaders<TModel, TOptions>(this IServiceProvider provider)
            where TModel : class, IExtensibleDataObject
            where TOptions : ExtensibleModelGetOptions
        {
            return new CompositeExtensibleModelReader<TModel, TOptions>(provider.GetServices<IExtensibleModelReader<TModel, TOptions>>);
        }

        public static IExtensibleModelValidator<TModel> ComposeExtensibleModelValidators<TModel>(this IServiceProvider provider)
            where TModel : class, IExtensibleDataObject
        {
            return new CompositeExtensibleModelValidator<TModel>(provider.GetServices<IExtensibleModelValidator<TModel>>);
        }

        public static IExtensibleModelCreator<TModel> ComposeExtensibleModelCreators<TModel>(this IServiceProvider provider)
            where TModel : class, IExtensibleDataObject
        {
            return new CompositeExtensibleModelCreator<TModel>(provider.GetServices<IExtensibleModelCreator<TModel>>);
        }

        public static IExtensibleModelUpdater<TModel> ComposeExtensibleModelUpdaters<TModel>(this IServiceProvider provider)
            where TModel : class, IExtensibleDataObject
        {
            return new CompositeExtensibleModelUpdater<TModel>(provider.GetServices<IExtensibleModelUpdater<TModel>>);
        }
    }
}
