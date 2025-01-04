using System.Diagnostics.CodeAnalysis;

namespace Hypercube.Core.IO.Prototypes.Storage;

public interface IPrototypeStorage
{
    T GetPrototype<T>(PrototypeId<T> id) where T : class, IPrototype;
    bool TryGetPrototype<T>(PrototypeId<T> id, [NotNullWhen(true)] out T? prototype) where T : class, IPrototype;
    bool HasPrototype<T>(PrototypeId<T> id) where T : class, IPrototype;
    IEnumerable<T> EnumeratePrototypes<T>() where T : class, IPrototype;
    void LoadPrototypes(string yamlData);
}