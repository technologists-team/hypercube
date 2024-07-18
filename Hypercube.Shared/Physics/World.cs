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
    private readonly HashSet<IBody> _bodies = new();
    
    public void Update(float deltaTime)
    {
        foreach (var bodyA in _bodies)
        {
            bodyA.Update(deltaTime);
            
            foreach (var bodyB in _bodies)
            {
                if (bodyA == bodyB)
                    continue;
                
                ProcessCollision(bodyA, bodyB);
            }
        }
        
        // TODO: Add works with chunks
        // ProcessChunkCollisions();
    }
    
    public void AddBody(IBody body)
    {
        _bodies.Add(body);
    }
    
    public void RemoveBody(IBody body)
    {
        _bodies.Remove(body);
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

    private void ProcessCollision(IBody bodyA, IBody bodyB)
    {
        if (TryProcessCircles(bodyA, bodyB))
            return;
        
        if (TryProcessRectangles(bodyA, bodyB))
            return;
    }

    private bool TryProcessCircles(IBody bodyA, IBody bodyB)
    {
        if (bodyA.Shape.Type != ShapeType.Circle || bodyB.Shape.Type != ShapeType.Circle)
            return false;

        if (!Collisions.IntersectsCircles(bodyA.ShapeCircle, bodyB.ShapeCircle, out var depth, out var normal))
            return false;
        
        bodyA.Move(normal * depth / 2f);
        bodyB.Move(-normal * depth / 2f);
        return true;
    }
    
    private bool TryProcessRectangles(IBody bodyA, IBody bodyB)
    {
        if (bodyA.Shape.Type != ShapeType.Rectangle || bodyB.Shape.Type != ShapeType.Rectangle)
            return false;

        if (!Collisions.IntersectsRectangles(bodyA.ShapeBox2, bodyB.ShapeBox2, out var depth, out var normal))
            return false;
        
        bodyA.Move(normal * depth / 2f);
        bodyB.Move(-normal * depth / 2f);
        return true;
    }
}