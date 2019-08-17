using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;

namespace graph_master.api.Helpers
{
    class DataContractInputFormatter : TextInputFormatter
    {
        public DataContractInputFormatter()
        {
            this.SupportedMediaTypes.Add("application/json");
            SupportedEncodings.Add(Encoding.UTF8);
            SupportedEncodings.Add(Encoding.Unicode);
        }

        protected override bool CanReadType(Type type)
        {
            if (type.IsArray)
                type = type.GetElementType();
            else if (type.IsGenericType && typeof(IEnumerable<>).IsAssignableFrom(type.GetGenericTypeDefinition()))
                type = type.GetGenericArguments()[0];

            return type.GetCustomAttribute<DataContractAttribute>() != null && type.GetInterface(nameof(IExtensibleDataObject)) != null;
        }

        public async override Task<InputFormatterResult> ReadRequestBodyAsync(InputFormatterContext context, Encoding selectedEncoding)
        {
            var serializationStrategies = DataContractSerializationStrategies.Create(context.ModelType);

            string content;
            using (var reader = new StreamReader(context.HttpContext.Request.Body, selectedEncoding))
            {
                Task<string> contentReading = reader.ReadToEndAsync();
                content = await contentReading;
            }

            using (MemoryStream stream = new MemoryStream())
            {
                stream.Write(Encoding.UTF8.GetBytes(content));
                content = null;
                stream.Position = 0;

                object composedInstance = serializationStrategies.ComposedModelJsonSerializer.ReadObject(stream);
                var composedInstances = composedInstance as IEnumerable;
                if (composedInstances != null)
                {
                    foreach (var item in composedInstances)
                    {
                        var extensibleInstance = item as IExtensibleDataObject;
                        if (extensibleInstance != null)
                            extensibleInstance.ExtensionData = null;
                    }
                }
                else
                {
                    var extensibleInstance = composedInstance as IExtensibleDataObject;
                    if (extensibleInstance != null)
                        extensibleInstance.ExtensionData = null;
                }

                stream.SetLength(0);
                serializationStrategies.ComposedModelXmlSerializer.WriteObject(stream, composedInstance);
                stream.Position = 0;
                object model = serializationStrategies.BaseModelSerializer.ReadObject(stream);
                return await InputFormatterResult.SuccessAsync(model);
            }
        }
    }


    class DataContractOutputFormatter : TextOutputFormatter
    {
        public DataContractOutputFormatter()
        {
            SupportedMediaTypes.Add("application/json");
            SupportedEncodings.Add(Encoding.UTF8);
            SupportedEncodings.Add(Encoding.Unicode);
        }

        protected override bool CanWriteType(Type type)
        {
            if (type.IsArray)
                type = type.GetElementType();
            else if (type.IsGenericType && typeof(IEnumerable<>).IsAssignableFrom(type.GetGenericTypeDefinition()))
                type = type.GetGenericArguments()[0];

            return type.GetCustomAttribute<DataContractAttribute>() != null && type.GetInterface(nameof(IExtensibleDataObject)) != null;
        }

        public async override Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding)
        {
            var serializationStrategies = DataContractSerializationStrategies.Create(context.ObjectType);

            using (var stream = new MemoryStream())
            {
                var items = context.Object as IEnumerable;
                if (items != null)
                {
                    var composedInstances = serializationStrategies.ComposedItemsCollectionFactory();
                    foreach (var item in items)
                    {
                        serializationStrategies.BaseItemXmlSerializer.WriteObject(stream, item);
                        stream.Position = 0;

                        // extra cycle to handle incorrect collection serialization
                        var composedInstance = serializationStrategies.ComposedItemXmlDeserializer.Deserialize(stream);
                        stream.SetLength(0);
                        composedInstances.Add(composedInstance);
                    }
                    stream.SetLength(0);
                    serializationStrategies.ComposedModelJsonSerializer.WriteObject(stream, composedInstances);
                }
                else
                {
                    serializationStrategies.BaseItemXmlSerializer.WriteObject(stream, context.Object);
                    stream.Position = 0;

                    // extra cycle to handle incorrect collection serialization
                    var composedInstance = serializationStrategies.ComposedItemXmlDeserializer.Deserialize(stream);
                    stream.SetLength(0);
                    serializationStrategies.ComposedModelJsonSerializer.WriteObject(stream, composedInstance);
                }

                stream.Position = 0;
                using (StreamReader reader = new StreamReader(stream))
                {
                    string json = reader.ReadToEnd();
                    await context.HttpContext.Response.WriteAsync(json, selectedEncoding);
                }
            }
        }
    }

    struct DataContractSerializationStrategies
    {
        /// <summary>
        /// Full model works both with collection and single item contracts
        /// It is most boundary strategy which is correctly maps to and from JSON into a composed models.
        /// It is responsible for controlling that input and output does not contain junky data.
        /// Note about model composition. It is the process of gathering all inherited extension models.
        /// It is triggered only for models which are annotated with DataContractAttribute and implements
        /// IExtensibleDataObject. Composed type of the model is built once and then is cached.
        /// </summary>
        public DataContractJsonSerializer ComposedModelJsonSerializer { get; private set; }

        /// <summary>
        /// Works only on input. It is the second strategy that is responsible for conversion into xml 
        /// the model instance that had just filtered and binded on first stage. Xml here is used
        /// due to it is native for data contract serialization tools and works completely without
        /// artifical adaptation like for its json variation.
        /// </summary>
        public DataContractSerializer ComposedModelXmlSerializer { get; private set; }

        /// <summary>
        /// Works only on input. It is the last stage of input models binding.
        /// It is responsible for packing extending parts into ExtensionData of extensible model instance
        /// to be later round tripped among specific strategies.
        /// </summary>
        public DataContractSerializer BaseModelSerializer { get; private set; }

        /// <summary>
        /// Works only on output and only when the model is a collection of extensible models.
        /// It is responsible for creation of generic List<T> whre T is a composed model.
        /// The list is needed later to correctly serialize an array and also to simplify
        /// conversion flow from xml to composed model instances by working on them one by one.
        /// </summary>
        public Func<IList> ComposedItemsCollectionFactory { get; private set; }

        /// <summary>
        /// Works only on output. It is the first stage of output model serialization.
        /// It is responsible for regular serialization of returned extensible model.
        /// For collection results it is invoked for every item separately.
        /// </summary>
        public DataContractSerializer BaseItemXmlSerializer { get; private set; }

        /// <summary>
        /// Works only on output. It is the second stage of output model serialization but not last.
        /// The last stage is handled by ComposedModelJsonSerializer like for inputs.
        /// This serialization strategy is responsible for building of composed model instance from 
        /// just prepared xml to be correctly serialized into the ouput json with completely known
        /// model on the last stage.
        /// </summary>
        public XmlSerializer ComposedItemXmlDeserializer { get; private set; }

        private const string ComposedNamespace = "fox.datasimple.ComposedModels";
        private static readonly ModuleBuilder ModuleBuilder = CreateModuleBuilder();
        private static readonly Dictionary<Type, Type> composedTypes = new Dictionary<Type, Type>();
        public static readonly DataContractJsonSerializerSettings Settings = new DataContractJsonSerializerSettings()
        {
            DateTimeFormat = new DateTimeFormat("u", CultureInfo.InvariantCulture)
        };

        private static readonly ConcurrentDictionary<Type, DataContractSerializationStrategies> ComposedSerializers =
            new ConcurrentDictionary<Type, DataContractSerializationStrategies>();

        public static DataContractSerializationStrategies Create(Type objectType)
        {
            return ComposedSerializers.GetOrAdd(objectType, CreateSerializationStrategies);
        }

        private static DataContractSerializationStrategies CreateSerializationStrategies(Type modelType)
        {
            Type collectionItemType = null;
            if (modelType.IsArray)
            {
                collectionItemType = modelType.GetElementType();
            }
            else if (modelType.IsGenericType)
            {
                IEnumerable<Type> interfaces = modelType.GetInterfaces();
                if (modelType.IsInterface)
                    interfaces = new[] { modelType }.Concat(interfaces);

                collectionItemType = interfaces
                    .Where(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                    .Select(x => x.GetGenericArguments()[0])
                    .FirstOrDefault();
            }

            Type composedType;
            Type composedItemType = null;
            if (collectionItemType != null)
            {
                composedItemType = GetComposedType(collectionItemType);
                composedType = typeof(List<>).MakeGenericType(composedItemType);
            }
            else
            {
                composedType = GetComposedType(modelType);
            }

            return new DataContractSerializationStrategies
            {
                BaseModelSerializer = new DataContractSerializer(modelType),
                ComposedModelJsonSerializer = new DataContractJsonSerializer(composedType, Settings),
                ComposedModelXmlSerializer = new DataContractSerializer(composedType),
                ComposedItemXmlDeserializer = new XmlSerializer(composedItemType ?? composedType),
                BaseItemXmlSerializer = new DataContractSerializer(collectionItemType ?? modelType),
                ComposedItemsCollectionFactory = collectionItemType != null ?
                    new Func<IList>(() => (IList)Activator.CreateInstance(composedType)) :
                    null
            };
        }

        private static Type GetComposedType(Type modelType)
        {
            Type dataContractAttrType = typeof(DataContractAttribute);
            var dataContractAttr = modelType.GetCustomAttributes(dataContractAttrType).SingleOrDefault();
            if (dataContractAttr == null && !modelType.IsGenericType && !modelType.IsArray)
                return modelType;

            Type composedType;
            if (composedTypes.TryGetValue(modelType, out composedType))
                return composedType;

            if (dataContractAttr != null)
            {
                TypeBuilder typeBuilder = ModuleBuilder.DefineType(
                    $"{ComposedNamespace}.{modelType.Name}_{Guid.NewGuid().ToString("N")}",
                    TypeAttributes.Public |
                    TypeAttributes.Class |
                    TypeAttributes.AutoClass |
                    TypeAttributes.AnsiClass |
                    TypeAttributes.BeforeFieldInit |
                    TypeAttributes.AutoLayout
                );
                composedType = typeBuilder;
                composedTypes.Add(modelType, typeBuilder);

                typeBuilder.DefineDefaultConstructor(MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName);

                var dataContractProps = new PropertyInfo[]{
                    dataContractAttrType.GetProperty(nameof(DataContractAttribute.Name)),
                    dataContractAttrType.GetProperty(nameof(DataContractAttribute.Namespace)),
                    dataContractAttrType.GetProperty(nameof(DataContractAttribute.IsReference))
                };

                string tagName = dataContractProps[0].GetValue(dataContractAttr) as string ?? modelType.Name;
                string tagNamespace = dataContractProps[1].GetValue(dataContractAttr) as string ?? modelType.Namespace;

                var dataContractPropValues = new object[] {
                    tagName,
                    tagNamespace,
                    dataContractProps[2].GetValue(dataContractAttr),
                };

                CustomAttributeBuilder dataContractAttributeBuilder = new CustomAttributeBuilder(
                    dataContractAttrType.GetConstructor(Type.EmptyTypes),
                    new object[0],
                    dataContractProps,
                    dataContractPropValues
                );

                typeBuilder.SetCustomAttribute(dataContractAttributeBuilder);

                var xmlRootType = typeof(XmlTypeAttribute);

                var xmlRootProps = new[] {
                    xmlRootType.GetProperty(nameof(XmlTypeAttribute.TypeName)),
                    xmlRootType.GetProperty(nameof(XmlTypeAttribute.Namespace)),
                };

                var xmlRootPropValues = new[]
                {
                    tagName,
                    tagNamespace
                };

                CustomAttributeBuilder xmlTypeAttributeBuilder = new CustomAttributeBuilder(
                    xmlRootType.GetConstructor(Type.EmptyTypes),
                    new object[0],
                    xmlRootProps,
                    xmlRootPropValues
                );

                typeBuilder.SetCustomAttribute(xmlTypeAttributeBuilder);

                IEnumerable<Type> derivers = new[] { modelType };
                if (modelType.GetInterface(nameof(IExtensibleDataObject)) != null)
                {
                    derivers = derivers.Concat(
                        AppDomain.CurrentDomain.GetAssemblies()
                            .Where(a => a != ModuleBuilder.Assembly)
                            .SelectMany(a => a.GetTypes())
                            .Where(t => t.IsClass && t.IsSubclassOf(modelType))
                    );
                }

                foreach (Type deriver in derivers)
                {
                    foreach (PropertyInfo propInfo in deriver.GetProperties())
                    {
                        if (propInfo.DeclaringType == deriver)
                        {
                            // copying DataMemberAttribute values
                            var dataMemberAttr = (DataMemberAttribute)propInfo.GetCustomAttributes(typeof(DataMemberAttribute)).SingleOrDefault();
                            if (dataMemberAttr == null)
                                continue;

                            Type composedPropertyType = GetComposedType(propInfo.PropertyType);
                            AddProperty(propInfo, composedPropertyType, dataMemberAttr, typeBuilder);
                        }
                    }
                }

                composedType = typeBuilder.CreateType();
                composedTypes[modelType] = composedType;
            }
            else if (modelType.IsGenericType)
            {
                var genericArguments = modelType.GetGenericArguments();
                Type enumerableInterface = null;
                if (
                    genericArguments.Length == 1 &&
                    (
                        enumerableInterface = modelType.GetInterfaces()
                            .Where(x => x.IsGenericType && typeof(IEnumerable<>) == x.GetGenericTypeDefinition())
                            .SingleOrDefault()
                    ) != null
                )
                {
                    Type composedElementType = GetComposedType(genericArguments[0]);
                    if (!composedTypes.TryGetValue(modelType, out composedType))
                    {
                        composedType = composedElementType.MakeArrayType();
                        composedTypes.Add(modelType, composedType);
                    }
                }
                else
                {
                    var composedGenericArguments = new Type[genericArguments.Length];
                    for (int i = 0; i < genericArguments.Length; i++)
                        composedGenericArguments[i] = GetComposedType(genericArguments[i]);

                    if (!composedTypes.TryGetValue(modelType, out composedType))
                    {
                        composedType = modelType.GetGenericTypeDefinition()
                            .MakeGenericType(composedGenericArguments);
                        composedTypes.Add(modelType, composedType);
                    }
                }
            }
            else // if (modelType.IsArray)
            {
                Type composedElementType = GetComposedType(modelType.GetElementType());
                if (!composedTypes.TryGetValue(modelType, out composedType))
                {
                    composedType = composedElementType.MakeArrayType();
                    composedTypes.Add(modelType, composedType);
                }
            }

            return composedType;
        }

        private static void AddProperty(
            PropertyInfo propertyInfo,
            Type composedPropertyType,
            DataMemberAttribute dataMemberAttribute,
            TypeBuilder typeBuilder
        )
        {
            string propertyName = dataMemberAttribute.Name as string ?? propertyInfo.Name;

            // generating property
            PropertyBuilder propertyBuilder = typeBuilder.DefineProperty(propertyName, PropertyAttributes.HasDefault, composedPropertyType, null);

            Type dataMemberAttributeType = typeof(DataMemberAttribute);
            var dataMemberProps = new PropertyInfo[]
            {
                dataMemberAttributeType.GetProperty(nameof(DataMemberAttribute.Name)),
                dataMemberAttributeType.GetProperty(nameof(DataMemberAttribute.IsRequired)),
                dataMemberAttributeType.GetProperty(nameof(DataMemberAttribute.EmitDefaultValue))
            };

            var dataMemberPropValues = new object[] {
                propertyName,
                dataMemberAttribute.IsRequired,
                dataMemberAttribute.EmitDefaultValue
            };

            CustomAttributeBuilder dataMemberAttributeBuilder = new CustomAttributeBuilder(
                dataMemberAttributeType.GetConstructor(Type.EmptyTypes),
                new object[0],
                dataMemberProps,
                dataMemberPropValues
            );

            propertyBuilder.SetCustomAttribute(dataMemberAttributeBuilder);

            // generating private field
            FieldBuilder fieldBuilder = typeBuilder.DefineField("_" + propertyName, composedPropertyType, FieldAttributes.Private);
            CustomAttributeBuilder hideFieldAttributeBuilder = new CustomAttributeBuilder(
                typeof(DebuggerBrowsableAttribute).GetConstructor(new[] { typeof(DebuggerBrowsableState) }),
                new object[] { DebuggerBrowsableState.Never }
            );
            fieldBuilder.SetCustomAttribute(hideFieldAttributeBuilder);

            // generating public getter/setter
            MethodBuilder getPropMthdBldr = typeBuilder.DefineMethod("get_" + propertyName, MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig, composedPropertyType, Type.EmptyTypes);
            ILGenerator getIl = getPropMthdBldr.GetILGenerator();

            getIl.Emit(OpCodes.Ldarg_0);
            getIl.Emit(OpCodes.Ldfld, fieldBuilder);
            getIl.Emit(OpCodes.Ret);

            MethodBuilder setPropMthdBldr = typeBuilder.DefineMethod("set_" + propertyName,
                MethodAttributes.Public |
                MethodAttributes.SpecialName |
                MethodAttributes.HideBySig,
                null, new[] { composedPropertyType });

            ILGenerator setIl = setPropMthdBldr.GetILGenerator();
            Label modifyProperty = setIl.DefineLabel();
            Label exitSet = setIl.DefineLabel();

            setIl.MarkLabel(modifyProperty);
            setIl.Emit(OpCodes.Ldarg_0);
            setIl.Emit(OpCodes.Ldarg_1);
            setIl.Emit(OpCodes.Stfld, fieldBuilder);

            setIl.Emit(OpCodes.Nop);
            setIl.MarkLabel(exitSet);
            setIl.Emit(OpCodes.Ret);

            propertyBuilder.SetGetMethod(getPropMthdBldr);
            propertyBuilder.SetSetMethod(setPropMthdBldr);
        }

        private static ModuleBuilder CreateModuleBuilder()
        {
            AssemblyName assemblyName = new AssemblyName(ComposedNamespace);
            AssemblyBuilder asmBuilder = AssemblyBuilder.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
            ModuleBuilder moduleBuilder = asmBuilder.DefineDynamicModule(ComposedNamespace);
            return moduleBuilder;
        }
    }
}