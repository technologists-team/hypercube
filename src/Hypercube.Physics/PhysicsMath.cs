using Hypercube.Physics.Shapes.Structs;
using JetBrains.Annotations;

namespace Hypercube.Physics;

[PublicAPI]
public static class PhysicsMath
{
    /// <summary>
    /// Half part of Separate Axis Theorem, checks only for polygon 1.
    /// </summary>
    public static float ComputeMaxSeparation(ref int edgeIndex, ref ShapePolygon polygon1, ref ShapePolygon polygon2)
    {
        var count1 = polygon1.Count;
        var count2 = polygon2.Count;
        
        var normals1 = polygon1.Normals.AsSpan();
        var vertices1 = polygon1.Vertices.AsSpan();
        
        var vertices2 = polygon2.Vertices.AsSpan();

        var bestIndex = 0;
        var maxSeparation = float.MinValue;
        
        for (var i = 0; i < count1; i++)
        {
            var normal = normals1[i];
            var vertex = vertices1[i];
            
            var minProjection = float.MaxValue;
            
            for (var j = 0; j < count2; j++)
            {
                var projected = Vector2.Dot(normal, vertices2[j] - vertex);
                if (projected < minProjection)
                    minProjection = projected;
            }

            if (minProjection <= maxSeparation)
                continue;
            
            maxSeparation = minProjection;
            bestIndex = i;
        }

        edgeIndex = bestIndex;
        return maxSeparation;
    }

    public static void ComputeBestSeparatingEdge(ref int edgeIndexA, ref int edgeIndexB, ref ShapePolygon polygonA, ref ShapePolygon polygonB, bool flip)
    {
        if (flip)
        {
            ComputeBestSeparatingEdge(ref edgeIndexB, ref polygonB, polygonA.Normals[edgeIndexA]);
            return;
        }
        
        ComputeBestSeparatingEdge(ref edgeIndexA, ref polygonA, polygonB.Normals[edgeIndexB]);
    }

    public static void ComputeBestSeparatingEdge(ref int edgeIndex, ref ShapePolygon polygon, Vector2 searchDirection)
    {
        edgeIndex = 0;
        
        var normals = polygon.Normals.AsSpan();
        var minDistance = float.MaxValue;
        var count = polygon.Count;
        for (var i = 0; i < count; i++)
        {
            var distance = Vector2.Dot(searchDirection, normals[i]);
            if (distance >= minDistance)
                continue;
                
            minDistance = distance;
            edgeIndex = i;
        }
    }
}