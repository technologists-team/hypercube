using System.Runtime.CompilerServices;

namespace Hypercube.Graphics.Core.Geometry;

public readonly struct VertexIndexPair
{
    public int Vertex { get; init; }
    public int Index { get; init; }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static VertexIndexPair GetCount(VertexIndexPair start, VertexIndexPair end)
        => end - start;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static VertexIndexPair operator-(VertexIndexPair pairA, VertexIndexPair pairB)
    {
        return new VertexIndexPair
        {
            Vertex = pairA.Vertex - pairB.Vertex,
            Index = pairA.Index - pairB.Index
        };
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static VertexIndexPair operator+(VertexIndexPair pairA, VertexIndexPair pairB)
    {
        return new VertexIndexPair
        {
            Vertex = pairA.Vertex + pairB.Vertex,
            Index = pairA.Index + pairB.Index
        };
    }

    public void Deconstruct(out int vertex, out int index)
    {
        vertex = Vertex;
        index = Index;
    }
    
    public override string ToString() => $"[Vertex: {Vertex}, Index: {Index}]";
}
