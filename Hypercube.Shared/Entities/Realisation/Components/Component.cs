namespace Hypercube.Shared.Entities.Realisation.Components;

public abstract class Component : IComponent
{
    public EntityUid Owner { get; set; }
}