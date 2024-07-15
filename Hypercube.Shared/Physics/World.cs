using Hypercube.Math.Vectors;
using Hypercube.Shared.Scenes;

namespace Hypercube.Shared.Physics;

/// <summary>
/// Analogous to a <see cref="Scene"/> in a physical representation.
/// Contains <see cref="Body"/> that will handle their physical interactions.
/// </summary>
/// <seealso cref="Chunk"/>
public sealed class World
{
    private readonly Dictionary<Vector2Int, Chunk> _chunks = new();

    public void Update(float deltaTime)
    {
        ProcessChunkCollisions();
    }

    public void AddBody(Body body)
    {
        
    }
    
    private void ProcessChunkCollisions()
    {
        foreach (var (position, chunk) in _chunks)
        {
            if (!chunk.ContainBodes)
                continue;

            for (var x = position.X; x < position.X + 3; x++)
            {
                for (var y = position.Y; y < position.Y + 3; y++)
                {
                    var otherPosition = new Vector2Int(x, y);
                   
                    if (!_chunks.TryGetValue(otherPosition, out var otherChunk))
                        continue;

                    if (!otherChunk.ContainBodes)
                        continue;
                    
                    ProcessChunkCollision(chunk, otherChunk);
                }
            }
        }
    }
    
    private void ProcessChunkCollision(Chunk chunkA, Chunk chunkB)
    {
        foreach (var bodyA in chunkA.Bodies)
        {
            foreach (var bodyB in chunkB.Bodies)
            {
                if (bodyA == bodyB)
                    continue;
                
                ProcessCollision(bodyA, bodyB);
            }
        }
    }

    private void ProcessCollision(Body bodyA, Body bodyB)
    {
        
    }
}