using System.Collections.Frozen;
using Hypercube.Graphics.Core.Attributes;
using Hypercube.Graphics.Types;
using Hypercube.Utilities.Helpers;

namespace Hypercube.Graphics.Backend;

public static class BackendFactory
{
    private static readonly FrozenDictionary<BackendType, Func<IBackend>> Backends;
    
    static BackendFactory()
    {
        var backend = new Dictionary<BackendType, Func<IBackend>>();
        foreach (var (type, attribute) in ReflectionHelper.GetAllTypesWithAttribute<BackendAttribute>())
        {
            backend[attribute.Backend] = () =>
                (IBackend) (type.GetConstructor([])?.Invoke(null) ?? throw new InvalidOperationException());
        }
        
        Backends = backend.ToFrozenDictionary();
    }
    
    public static IBackend Create(BackendType type) => Backends[type].Invoke();
}
