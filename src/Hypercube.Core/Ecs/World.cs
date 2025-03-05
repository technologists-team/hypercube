using Hypercube.Core.Ecs.Systems;
using Hypercube.Utilities.Helpers;

namespace Hypercube.Core.Ecs;

public class World : IWorld
{
    private readonly Dictionary<Type, IEntitySystem> _systems = [];

    public void Update(float deltaTime)
    {
        foreach (var (_, system) in _systems)
            system.Update(deltaTime, this);
    }

    public bool AddSystem<T>() where T : IEntitySystem
    {
        if (_systems.ContainsKey(typeof(T)))
            return false;

        _systems.Add(typeof(T), InstantiateSystem<T>());
        return true;
    }

    public IEntitySystem GetSystem<T>() where T : IEntitySystem
    {
        return _systems[typeof(T)];
    }

    private T InstantiateSystem<T>() where T : IEntitySystem
    {
        var constructors = typeof(T).GetConstructors();
        if (constructors.Length == 0)
            throw new Exception();

        // it's not assumed that systems have a meaningful constructor
        var constructor = constructors[0];
        var instance = constructor.Invoke(null);
        
        // Since we are working with an interface we cannot use a constructor.
        // I don't want to create an initialization method and allow nullable types either.
        // So we just set the value to getter.
        ReflectionHelper.SetField(instance, nameof(IEntitySystem.World), this);

        return (T) instance;
    }
}
