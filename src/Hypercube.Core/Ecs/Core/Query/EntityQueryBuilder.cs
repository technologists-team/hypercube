namespace Hypercube.Core.Ecs.Core.Query;

public sealed class EntityQueryBuilder
{
    private readonly World _world;
    private readonly List<ComponentQueryData> _requiredComponents = [];
    private readonly List<ComponentQueryData> _excludedComponents = [];
    
    public EntityQueryBuilder(World world)
    {
        _world = world;
    }
    
    public EntityQueryBuilder With<T>()
        where T : IComponent
    {
        _requiredComponents.Add(new ComponentQueryData(typeof(T), _world.GetComponentMapper<T>()));
        return this;
    }

    public EntityQueryBuilder Without<T>()
        where T : IComponent
    {
        _excludedComponents.Add(new ComponentQueryData(typeof(T), _world.GetComponentMapper<T>()));
        return this;
    }

    public EntityQuery Build()
    {
        return new EntityQuery(_world, _requiredComponents.ToArray(), _excludedComponents.ToArray());
    }
}