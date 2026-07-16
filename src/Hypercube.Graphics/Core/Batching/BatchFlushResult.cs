using Hypercube.Graphics.Core.Types;

namespace Hypercube.Graphics.Core.Batching;

public readonly struct BatchFlushResult
{
    public static readonly BatchFlushResult Empty = new BatchFlushResult
    {
        Vertices = [],
        Indices = [],
        Groups = []
    }; 
    
    public Vertex[] Vertices { get; init; }
    public uint[] Indices { get; init; }
    public IReadOnlyList<BatchGroup> Groups { get; init; }
}