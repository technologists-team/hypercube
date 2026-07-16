namespace Hypercube.Graphics.Core.Geometry;

public readonly struct GeometryChunk
{
    public VertexIndexPair Start { get; init; }
    public VertexIndexPair End { get; init; }
    
    public VertexIndexPair Count => VertexIndexPair.GetCount(Start, End);
}
