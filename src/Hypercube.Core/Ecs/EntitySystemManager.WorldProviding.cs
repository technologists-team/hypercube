using Hypercube.Ecs;
using Hypercube.Ecs.Components;
using Hypercube.Ecs.Events;
using Hypercube.Ecs.Queries;

namespace Hypercube.Core.Ecs;

public partial class EntitySystemManager
{
    public IEventBus Events => _globalWorld.Events;
    
    public Entity Create() => _globalWorld.Create();

    public void Delete(Entity entity) => _globalWorld.Delete(entity);

    public bool Validate(Entity entity) => _globalWorld.Validate(entity);

    public ref T Add<T>(Entity entity) where T : struct, IComponent
        => ref _globalWorld.Add<T>(entity);

    public ref T Add<T>(Entity entity, in T component) where T : struct, IComponent
        => ref _globalWorld.Add(entity, in component);

    public ref T Get<T>(Entity entity) where T : struct, IComponent
        => ref _globalWorld.Get<T>(entity) ;

    public bool Has<T>(Entity entity) where T : struct, IComponent
        => _globalWorld.Has<T>(entity);

    public void Remove<T>(Entity entity) where T : struct, IComponent
        => _globalWorld.Remove<T>(entity);

    public Query Query(in QueryMeta meta)
        => _globalWorld.Query(meta);
}