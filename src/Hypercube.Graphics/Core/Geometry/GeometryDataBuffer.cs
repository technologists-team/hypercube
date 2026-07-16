using Hypercube.Graphics.Core.Types;

namespace Hypercube.Graphics.Core.Geometry;

public sealed class GeometryDataBuffer
{
    private const float GrowthFactor = 2.0f;
    private const float ShrinkThreshold = 0.5f;
    private const float ShrinkFactor = 1.5f;
    private const int ShrinkAfterFrames = 2000;

    private Vertex[] _vertices = new Vertex[Constants.BufferVerticesSize];
    private uint[] _indices = new uint[Constants.BufferIndicesSize];

    private int _vertexCursor;
    private int _indexCursor;
    
    private int _peakVertexUsage;
    private int _peakIndexUsage;
    
    private int _lowUsageFrameCount;
    
    public int VertexCount => _vertexCursor;
    public int IndexCount => _indexCursor;
    
    public int VertexCapacity => _vertices.Length;
    public int IndexCapacity => _indices.Length;
    
    public ReadOnlySpan<Vertex> GetVertices() => _vertices.AsSpan(0, _vertexCursor);
    public ReadOnlySpan<uint> GetIndices() => _indices.AsSpan(0, _indexCursor);
    
    public uint PushVertex(Vertex vertex)
    {
        EnsureBuffer(ref _vertices, _vertexCursor, 1);
        
        var index = (uint) _vertexCursor;
        
        _vertices[_vertexCursor++] = vertex;
        
        UpdatePeak(ref _peakVertexUsage, _vertexCursor);
        return index;
    }
    
    public uint PushVertices(ReadOnlySpan<Vertex> vertices)
    {
        EnsureBuffer(ref _vertices, _vertexCursor, vertices.Length);
        
        var baseIndex = (uint) _vertexCursor;
        
        vertices.CopyTo(_vertices.AsSpan(_vertexCursor));
        _vertexCursor += vertices.Length;
        
        UpdatePeak(ref _peakVertexUsage, _vertexCursor);
        return baseIndex;
    }
    
    public void PushIndex(uint localIndex, uint baseVertex)
    {
        EnsureBuffer(ref _indices, _indexCursor, 1);
        
        _indices[_indexCursor++] = localIndex + baseVertex;
        UpdatePeak(ref _peakIndexUsage, _indexCursor);
    }
    
    public void PushIndices(ReadOnlySpan<uint> localIndices, uint baseVertex)
    {
        EnsureBuffer(ref _indices, _indexCursor, localIndices.Length);
        
        if (baseVertex == 0)
        {
            localIndices.CopyTo(_indices.AsSpan(_indexCursor));
            _indexCursor += localIndices.Length;
                    
            UpdatePeak(ref _peakIndexUsage, _indexCursor);
            return;
        }

        foreach (var t in localIndices)
            _indices[_indexCursor++] = t + baseVertex;

        UpdatePeak(ref _peakIndexUsage, _indexCursor);
    }
    
    public void Reset()
    {
        Shrink();
        
        _vertexCursor = 0;
        _indexCursor = 0;
    }

    private void Shrink()
    {
        var vertexUsage = _peakVertexUsage;
        var indexUsage = _peakIndexUsage;
        
        _peakVertexUsage = 0;
        _peakIndexUsage = 0;
        
        if (CheckUsage(vertexUsage, _vertices.Length) &&
            CheckUsage(indexUsage, _indices.Length))
            return;
        
        if (++_lowUsageFrameCount < ShrinkAfterFrames)
            return;
        
        _lowUsageFrameCount = 0;
                    
        ShrinkBuffer(ref _vertices, vertexUsage, Constants.MinVertexCapacity);
        ShrinkBuffer(ref _indices, indexUsage, Constants.MinIndexCapacity);
    }
    
    private static void EnsureBuffer<T>(ref T[] array, int currentCursor, int additionalCount)
    {
        if (currentCursor + additionalCount <= array.Length)
            return;
            
        var newCapacity = int.Max((int) (array.Length * GrowthFactor), currentCursor + additionalCount);
        Array.Resize(ref array, newCapacity);
    }

    private static void ShrinkBuffer<T>(ref T[] array, int peakUsage, int minCapacity)
    {
        var newCapacity = int.Max(minCapacity, (int) (peakUsage * ShrinkFactor));
        if (newCapacity >= array.Length)
            return;
        
        Array.Resize(ref array, newCapacity);
    }

    private static void UpdatePeak(ref int peak, int current)
    {
        if (current > peak)
            peak = current;
    }

    private static bool CheckUsage(int peakUsage, int capacity)
    {
        return (float) peakUsage / capacity < ShrinkThreshold;
    }
}
