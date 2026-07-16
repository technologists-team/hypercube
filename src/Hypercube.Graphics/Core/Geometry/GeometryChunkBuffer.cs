namespace Hypercube.Graphics.Core.Geometry;

public class GeometryChunkBuffer
{
    private GeometryChunk[] _references = new GeometryChunk[64]; 
    private int _referencesCursor;
    
    public ReadOnlySpan<GeometryChunk> References 
        => _references.AsSpan(0, _referencesCursor);

    public void Push(GeometryChunk chunk)
    {
        if (_referencesCursor >= _references.Length)
            Array.Resize(ref _references, _references.Length * 2);
        
        _references[_referencesCursor++] = chunk;
    }

    public void Reset()
    {
        _referencesCursor = 0;
    }
}
