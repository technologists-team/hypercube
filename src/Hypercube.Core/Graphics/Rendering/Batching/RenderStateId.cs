using Hypercube.Utilities.Attributes;

namespace Hypercube.Core.Graphics.Rendering.Batching;

[IdStruct(typeof(int))]
public readonly partial struct RenderStateId
{
    public static readonly RenderStateId Zero = new(0);
}
