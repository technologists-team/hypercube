using Hypercube.Utilities.Attributes;

namespace Hypercube.Core.Systems.Map;

[IdStruct(typeof(int))]
public readonly partial struct MapId
{
    public static readonly MapId Invalid = new(-1);
}
