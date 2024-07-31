using Hypercube.Math.Shapes;
using Hypercube.Math.Vectors;

namespace Hypercube.Shared.Physics;

public interface IShape
{
    ShapeType Type { get; }
    
    float Radius { get; }
    Vector2 Position { get; }
    
    int VerticesCount { get; }
    Vector2[] Vertices { get; }
    
    Vector2[] GetVerticesTransformed(Vector2 position, float rotation);
    Box2 ComputeAABB(Vector2 position, float rotation);
}