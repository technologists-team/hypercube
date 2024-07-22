using System.Collections.Frozen;
using System.Reflection;
using Hypercube.Shared.Dependency;
using Hypercube.Shared.EventBus;
using Hypercube.Shared.Resources.Manager;
using Hypercube.Shared.Runtimes.Event;
using Hypercube.Shared.Utilities.Helpers;
using YamlDotNet.RepresentationModel;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Hypercube.Shared.Resources.Data;

public sealed class ResourceDataContainer : IResourceDataContainer, IEventSubscriber, IPostInject
{
    [Dependency] private readonly IResourceLoader _resourceLoader = default!;
    [Dependency] private readonly IEventBus _eventBus = default!;
    
    private readonly FrozenDictionary<Type, Dictionary<string, IResourceData>> _data;
    private readonly FrozenDictionary<string, Type> _dataTypes;

    public ResourceDataContainer()
    {
        var data = new Dictionary<Type, Dictionary<string, IResourceData>>();
        var dataTypes = new Dictionary<string, Type>();
        
        foreach (var type in ReflectionHelper.GetAllInstantiableSubclassOf<IResourceData>())
        {
            data[type] = new Dictionary<string, IResourceData>();
            dataTypes[type.Name] = type;
        }
        
        _data = data.ToFrozenDictionary();
        _dataTypes = dataTypes.ToFrozenDictionary();
    }

    public void PostInject()
    {
        _eventBus.Subscribe<RuntimeStartupEvent>(this, OnStartup);
    }

    private void OnStartup(ref RuntimeStartupEvent args)
    {
        Load();
    }

    private void Load()
    {
        var deserializer = new DeserializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .Build();
        
        var method = GetType().GetMethod(nameof(deserializer.Deserialize), BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        
        foreach (var path in _resourceLoader.FindContentFiles("/Data/"))
        {
            using var stream = _resourceLoader.ReadFileContent(path) ?? throw new NullReferenceException();
            using var readerStream = _resourceLoader.WrapStream(stream);

            var yamlStream = new YamlStream();
            yamlStream.Load(readerStream);

            foreach (var document in yamlStream.Documents)
            {
                var root = (YamlMappingNode)document.RootNode[0];
                var typeNode = (YamlScalarNode)root["type"];
                var typeValue = typeNode.Value ?? throw new InvalidOperationException();
                var type = _dataTypes[typeValue];
                
                var genericMethod = method.MakeGenericMethod(type);
            }
        }
    }
}