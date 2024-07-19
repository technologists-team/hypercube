using Hypercube.Math.Shapes;
using Hypercube.Math.Vectors;

namespace Hypercube.Shared.Physics;

public static class Collisions
{
    public static bool IntersectsPolygon(Vector2[] verticesA, Vector2[] verticesB, out float depth, out Vector2 normal)
    {
        normal = Vector2.Zero;
        depth = float.MaxValue;

        for (var i = 0; i < verticesA.Length; i++)
        {
            var va = verticesA[i];
            var vb = verticesA[(i + 1) % verticesA.Length];

            var edge = vb - va;
            var axis = new Vector2(-edge.Y, edge.X).Normalized;
            
            ProjectVertices(verticesA, axis, out var minA, out var maxA);
            ProjectVertices(verticesB, axis, out var minB, out var maxB);

            if (minA >= maxB || minB >= maxA)
                return false;

            var axisDepth = MathF.Min(maxB - minA, maxA - minB);
            if (axisDepth >= depth)
                continue;
            
            depth = axisDepth;
            normal = axis;
        }
        
        for (var i = 0; i < verticesB.Length; i++)
        {
            var va = verticesB[i];
            var vb = verticesB[(i + 1) % verticesB.Length];

            var edge = vb - va;
            var axis = new Vector2(-edge.Y, edge.X).Normalized;

            ProjectVertices(verticesA, axis, out var minA, out var maxA);
            ProjectVertices(verticesB, axis, out var minB, out var maxB);

            if (minA >= maxB || minB >= maxA)
                return false;

            var axisDepth = MathF.Min(maxB - minA, maxA - minB);
            if (axisDepth >= depth)
                continue;
            
            depth = axisDepth;
            normal = axis;
        }

        var centerA = GetPolygonCenter(verticesA);
        var centerB = GetPolygonCenter(verticesB);
        var direction = centerB - centerA;
        
        if (Vector2.Dot(direction, normal) < 0f)
            normal = -normal;

        return true;
    }
    
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
    
    public static bool IntersectsRectangles(Box2 a, Box2 b, out float depth, out Vector2 normal)
    {
        depth = 0;
        normal = Vector2.Zero;
        
        if (b.Point0.Y > a.Point1.Y ||
            b.Point1.Y < a.Point0.Y ||
            b.Point1.X < a.Point0.X ||
            b.Point0.X > a.Point1.X)
            return false;
        
        var intersects = new Box2(
            Vector2.Max(a.Point0, b.Point0),
            Vector2.Min(a.Point1, b.Point1)
        );
        
        var line = GetRectangleLineIntersection(intersects, a.Center, b.Center);

        depth = line.Length;
        normal = (a.Center - b.Center).Normalized;

        return true;
    }

    public static Box2 GetRectangleLineIntersection(Box2 rectangle, Vector2 lineA, Vector2 lineB)
    {
        var t0 = 0f;
        var t1 = 1f;
        
        var deltaLine = lineB - lineA;
        
        var p = 0f;
        var q = 0f;
        var r = 0f;

        for(var edge = 0; edge < 4; edge++) {
            switch (edge)
            {
                // Traverse through left, right, bottom, top edges.
                case 0:
                    p = -deltaLine.X;
                    q = -(rectangle.Point0.X - lineA.X);
                    break;
                
                case 1:
                    p =  deltaLine.X;
                    q =  (rectangle.Point1.X - lineA.X);
                    break;
                
                case 2:
                    p = -deltaLine.Y;
                    q = -(rectangle.Point0.Y - lineA.Y);
                    break;
                
                case 3:
                    p =  deltaLine.Y;
                    q =  (rectangle.Point1.Y - lineA.Y);
                    break;
            }

            r = q / p;

            if (p == 0 && q < 0)
                return Box2.NaN;   // Don't draw line at all. (parallel line outside)

            if(p < 0)
            {
                if (r > t1)
                    return Box2.NaN;     // Don't draw line at all.
                
                if (r > t0)
                    t0 = r;     // Line is clipped!
                
                continue;
            }

            if (p > 0)
            {
                if(r < t0)
                    return Box2.NaN;      // Don't draw line at all.
                
                if (r < t1)
                    t1 = r;     // Line is clipped!
            }
        }

        return new Box2(
            lineA.X + t0 * deltaLine.X,
            lineA.Y + t0 * deltaLine.Y,
            lineA.X + t1 * deltaLine.X,
            lineA.Y + t1 * deltaLine.Y);
    }
    
    private static void ProjectVertices(Vector2[] vertices, Vector2 axis, out float min, out float max)
    {
        min = float.MaxValue;
        max = float.MinValue;

        foreach (var v in vertices)
        {
            var proj = Vector2.Dot(v, axis);

            if (proj < min)
                min = proj;
            
            if (proj > max)
                max = proj;
        }
    }

    private static Vector2 GetPolygonCenter(Vector2[] vertices)
    {
        var sum = Vector2.Zero;
        
        foreach (var vertex in vertices)
        {
            sum += vertex;
        }

        return sum / vertices.Length;
    }
}