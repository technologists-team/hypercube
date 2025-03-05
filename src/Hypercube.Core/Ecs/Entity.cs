using Hypercube.Core.Ecs.Components;

namespace Hypercube.Core.Ecs;

public readonly struct Entity
{
    public readonly int Id;

    public Entity(int id)
    {
        Id = id;
    }
}

public readonly struct Entity<T> where T : IComponent
{
    public readonly int Id;
    public readonly T Component;

    public Entity(Entity entity, T component)
    {
        Id = entity.Id;
        Component = component;
    }
}