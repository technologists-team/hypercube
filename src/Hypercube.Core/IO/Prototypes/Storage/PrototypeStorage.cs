using System.Collections.Frozen;
using System.Diagnostics.CodeAnalysis;
using Hypercube.Core.IO.Parsers.Yaml;
using Hypercube.Utilities.Helpers;

namespace Hypercube.Core.IO.Prototypes.Storage;

public class PrototypeStorage : IPrototypeStorage
{
    private readonly FrozenDictionary<string, Type> _types;
    private readonly Dictionary<string, IPrototype> _prototypes = new();

    public PrototypeStorage()
    {
        var types = new Dictionary<string, Type>();
        foreach (var (type, attribute) in ReflectionHelper.GetAllTypesWithAttribute<PrototypeAttribute>())
        {
            types[attribute.Id] = type;
        }

        _types = types.ToFrozenDictionary();
    }

    public T GetPrototype<T>(PrototypeId<T> id) where T : class, IPrototype
    {
        if (_prototypes.TryGetValue(id.Id, out var prototype) && prototype is T typedPrototype)
            return typedPrototype;

        throw new KeyNotFoundException($"Prototype with ID '{id.Id}' not found or is of the wrong type.");
    }

    public bool TryGetPrototype<T>(PrototypeId<T> id, [NotNullWhen(true)] out T? prototype) where T : class, IPrototype
    {
        if (_prototypes.TryGetValue(id.Id, out var proto) && proto is T typedPrototype)
        {
            prototype = typedPrototype;
            return true;
        }

        prototype = null;
        return false;
    }

    public bool HasPrototype<T>(PrototypeId<T> id) where T : class, IPrototype
    {
        return _prototypes.TryGetValue(id.Id, out var proto) && proto is T;
    }

    public IEnumerable<T> EnumeratePrototypes<T>() where T : class, IPrototype
    {
        return _prototypes.Values.OfType<T>();
    }
    
    public void LoadPrototypes(string yamlData)
    {
        var rawPrototypes = YamlParser.ParseYaml(yamlData);

        foreach (var (id, fields) in rawPrototypes)
        {
            if (!fields.TryGetValue("type", out var value))
                throw new InvalidOperationException($"Prototype '{id}' is missing a 'type' field.");

            if (!_types.TryGetValue(value, out var prototypeType))
                throw new InvalidOperationException($"Unknown or incompatible type '{value}' for prototype '{id}'.");

            if (Activator.CreateInstance(prototypeType) is not IPrototype prototype)
                throw new InvalidOperationException($"Failed to create prototype '{id}' of type '{value}'.");

            PopulateFields(prototype, fields);
            var property = prototypeType.GetProperty(nameof(IPrototype.Id));

            if (property is null)
                throw new InvalidOperationException($"Prototype '{id}' does not have a valid '{nameof(IPrototype.Id)}' property.");

            property.SetValue(prototype, id);
            _prototypes[id] = prototype;
        }
    }

    private void PopulateFields(IPrototype prototype, Dictionary<string, string> fields)
    {
        var type = prototype.GetType();
        foreach (var (fieldName, rawValue) in fields)
        {
            var property = type.GetProperty(fieldName);
            if (property is null || !property.CanWrite)
                continue;
            
            var convertedValue = Convert.ChangeType(rawValue, property.PropertyType);
            property.SetValue(prototype, convertedValue);
        }
    }
}