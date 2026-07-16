using Hypercube.Graphics.Core.Types;

namespace Hypercube.Graphics.Core.Geometry;

public class GeometryBatchBuffer
{
    public readonly GeometryDataBuffer Data = new();
    public readonly GeometryChunkBuffer Chunk = new();

    public void Push(ReadOnlySpan<Vertex> vertices, ReadOnlySpan<uint> indices)
    {
        var startVertex = Data.VertexCount;
        var startIndex = Data.IndexCount;
        
        var baseVertex = Data.PushVertices(vertices);
        
        Data.PushIndices(indices, baseVertex);
        
        var endVertex = Data.VertexCount;
        var endIndex = Data.IndexCount;
        
        var chunk = new GeometryChunk
        {
            Start = new VertexIndexPair { Vertex = startVertex, Index = startIndex },
            End = new VertexIndexPair { Vertex = endVertex, Index = endIndex }
        };

        Chunk.Push(chunk);
    }

    public void Reset()
    {
        Data.Reset();
        Chunk.Reset();
    }
}
