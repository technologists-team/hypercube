using Hypercube.Graphics.Core.Types;

namespace Hypercube.Graphics.Core.Batching;

public readonly struct Batch
{
    public Vertex[] Vertices { get; init; }
    public uint[] Indices { get; init; }
    public RenderState State { get; init; }
    public int Depth { get;  init; }
    public BatchFlags Flags { get;  init; }
}
