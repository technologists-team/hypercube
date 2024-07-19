using Hypercube.Math.Vectors;

namespace Hypercube.Shared.Physics;

/// <summary>
/// Chunks, is an optimization mechanism that allows us
/// to not process collisions with all objects in the current <see cref="World"/>.
/// When we need to, we process collisions with all
/// objects between two neighboring chunks.
/// </summary>
public sealed class Chunk
{
    public readonly Vector2Int Position;
    
    private HashSet<IBody> _bodies = new();
    
    public IReadOnlySet<IBody> Bodies => _bodies;
    public bool ContainBodes => _bodies.Count != 0;
    
    public Chunk(Vector2Int position)
    {
        Position = position;
    }
}