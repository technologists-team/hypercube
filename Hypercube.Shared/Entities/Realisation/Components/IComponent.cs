namespace Hypercube.Shared.Entities.Realisation.Components;

public interface IComponent
{
    EntityUid Owner { get; set; }
}