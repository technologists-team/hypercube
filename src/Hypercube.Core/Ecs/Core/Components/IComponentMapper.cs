namespace Hypercube.Core.Ecs.Core.Components;

[EngineInternal]
public interface IComponentMapper
{
    event Action<int>? Added; 
    event Action<int>? Removed; 
    bool Empty { get; }
    int Count { get; }
    IEnumerable<int> Entities { get; }
    bool Has(int entity);
}