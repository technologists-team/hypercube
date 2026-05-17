using Hypercube.Ecs;

namespace Hypercube.Core.Ecs;

public partial class EntitySystemManager
{
    public object Add(Entity entity, Type type)
    {
        return _globalWorld.Add(entity, type);
    }

    public void Add(Entity entity, object? value)
    {
        _globalWorld.Add(entity, value);
    }

    public object Get(Entity entity, Type type)
    {
        return _globalWorld.Get(entity, type);
    }

    public bool Has(Entity entity, Type type)
    {
        return _globalWorld.Has(entity, type);
    }

    public void Remove(Entity entity, Type type)
    {
        _globalWorld.Remove(entity, type);
    }
}