using Hypercube.Math.Shapes;
using Hypercube.Math.Vectors;

namespace Hypercube.Shared.Physics;

public static class Collisions
{
    public static bool IntersectsCircles(Circle a, Circle b)
    {
        var distance = Vector2.Distance(a.Position, b.Position);
        var radius = a.Radius + b.Radius;
        
        return distance >= radius;
    }
    
    public static bool IntersectsCircles(Circle a, Circle b, out float depth, out Vector2 normal)
    {
        var distance = Vector2.Distance(a.Position, b.Position);
        var radius = a.Radius + b.Radius;
        
        depth = radius - distance;
        normal = (a.Position - b.Position).Normalized;
        
        return distance < radius;
    }
    
    public static bool IntersectsRectangles(Box2 a, Box2 b)
    {
        return b.Point0.Y <= a.Point1.Y &&
               b.Point1.Y >= a.Point0.Y &&
               b.Point1.X >= a.Point0.X &&
               b.Point0.X <= a.Point1.X;
    }
}