using Hypercube.Mathematics.Shapes;
using Hypercube.Mathematics.Vectors;

namespace Hypercube.Shared.Physics;

public static class Collisions
{
    public static bool IntersectCirclePolygon(Circle circle, 
        Vector2 polygonCenter, Vector2[] vertices, out float depth, out Vector2 normal)
    {
        return IntersectCirclePolygon(circle.Position, circle.Radius, polygonCenter, vertices, out depth, out normal);
    }
    
    public static bool IntersectCirclePolygon(Vector2 circlePosition, float circleRadius, 
        Vector2 polygonCenter, Vector2[] vertices, out float depth, out Vector2 normal)
    {
        normal = Vector2.Zero;
        depth = float.MaxValue;

        Vector2 axis;
        float axisDepth;
        
        float minA;
        float maxA;
        float minB;
        float maxB;

        for (var i = 0; i < vertices.Length; i++)
        {
            var va = vertices[i];
            var vb = vertices[(i + 1) % vertices.Length];

            var edge = vb - va;
            axis = new Vector2(-edge.Y, edge.X).Normalized;

            ProjectVertices(vertices, axis, out minA, out maxA);
            ProjectCircle(circlePosition, circleRadius, axis, out minB, out maxB);

            if (minA >= maxB || minB >= maxA)
                return false;

            axisDepth = MathF.Min(maxB - minA, maxA - minB);
            if (axisDepth >= depth)
                continue;
            
            depth = axisDepth;
            normal = axis;
        }

        var cpIndex = FindClosestPointOnPolygon(circlePosition, vertices);
        var cp = vertices[cpIndex];

        axis = cp - circlePosition;
        axis = axis.Normalized;

        ProjectVertices(vertices, axis, out minA, out maxA);
        ProjectCircle(circlePosition, circleRadius, axis, out minB, out maxB);

        if (minA >= maxB || minB >= maxA)
            return false;
        

        axisDepth = MathF.Min(maxB - minA, maxA - minB);
        if (axisDepth < depth)
        {
            depth = axisDepth;
            normal = axis;
        }

        var direction = polygonCenter - circlePosition;
        if (Vector2.Dot(direction, normal) < 0f)
            normal = -normal;

        return true;
    }
    
    public static bool IntersectsPolygon(Vector2 centerA, Vector2[] verticesA, Vector2 centerB, Vector2[] verticesB, out float depth, out Vector2 normal)
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

        var direction = centerB - centerA;
        if (Vector2.Dot(direction, normal) < 0f)
            normal = -normal;

        return true;
    }
    
    public static bool IntersectsCircles(Circle circleA, Circle circleB)
    {
        var distance = Vector2.Distance(circleA.Position, circleB.Position);
        var radius = circleA.Radius + circleB.Radius;
        return distance >= radius;
    }
    
    public static bool IntersectsCircles(Circle circleA, Circle circleB, out float depth, out Vector2 normal)
    {
        return IntersectsCircles(circleA.Position, circleA.Radius, circleB.Position, circleB.Radius,
            out depth, out normal);
    }

    public static bool IntersectsCircles(Vector2 positionA, float radiusA, Vector2 positionB, float radiusB,
        out float depth, out Vector2 normal)
    {
        var distance = Vector2.Distance(positionA, positionB);
        var radius = radiusA + radiusB;
        
        depth = radius - distance;
        normal = -(positionA - positionB).Normalized;
        
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

            var r = q / p;

            if (p == 0 && q < 0)
                return Box2.NaN;   // Don't draw line at all. (parallel line outside)

            if (p < 0)
            {
                if (r > t1)
                    // Don't draw line at all
                    return Box2.NaN;
                
                if (r > t0)
                    // Line is clipped
                    t0 = r;
                
                continue;
            }

            if (p <= 0)
                continue;

            if (r < t0)
                // Don't draw line at all
                return Box2.NaN;
                
            if (r < t1)
                // Line is clipped
                t1 = r;
        }

        return new Box2(
            lineA.X + t0 * deltaLine.X,
            lineA.Y + t0 * deltaLine.Y,
            lineA.X + t1 * deltaLine.X,
            lineA.Y + t1 * deltaLine.Y);
    }
    
    
    private static int FindClosestPointOnPolygon(Vector2 circleCenter, Vector2[] vertices)
    {
        var result = -1;
        var minDistance = float.MaxValue;

        for (var i = 0; i < vertices.Length; i++)
        {
            var v = vertices[i];
            var distance = Vector2.Distance(v, circleCenter);

            if (distance >= minDistance)
                continue;
            
            minDistance = distance;
            result = i;
        }

        return result;
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
    
    private static void ProjectCircle(Circle circle, Vector2 axis, out float min, out float max)
    {
        ProjectCircle(circle.Position, circle.Radius, axis, out min, out max);
    }
    
    private static void ProjectCircle(Vector2 position, float radius, Vector2 axis, out float min, out float max)
    {
        var direction = axis.Normalized;
        var directionAndRadius = direction * radius;

        min = Vector2.Dot(position + directionAndRadius, axis);
        max = Vector2.Dot(position - directionAndRadius, axis);

        if (min <= max)
            return;
        
        // Swap the min and max values.
        (min, max) = (max, min);
    }
}