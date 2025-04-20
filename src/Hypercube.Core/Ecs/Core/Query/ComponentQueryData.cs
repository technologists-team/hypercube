using Hypercube.Core.Ecs.Core.Components;

namespace Hypercube.Core.Ecs.Core.Query;

[EngineInternal]
public readonly struct ComponentQueryData
{
    public readonly Type Type;
    public readonly IComponentMapper Mapper;

    public ComponentQueryData(Type type, IComponentMapper mapper)
    {
        Type = type;
        Mapper = mapper;
    }
}