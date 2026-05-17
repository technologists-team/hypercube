using Hypercube.Physics.Manifolds;
using Hypercube.Physics.Mathematics;
using Hypercube.Physics.Shapes.Structs;
using Hypercube.Utilities.Collections;
using JetBrains.Annotations;

namespace Hypercube.Physics;

[PublicAPI]
public static class Collide 
{
    public static Manifold Circles(ref ShapeCircle circleA, Transform transformA, ref ShapeCircle circleB, Transform transformB)
    {
        var transform = transformA.ToLocalSpaceOf(transformB);

        var pointA = circleA.Center;
        var pointB = transform.TransformPoint(pointA);
        var pointDirection = pointB - pointA;

        // NOTE: Maybe LengthFast?
        var (distance, normal) = pointDirection.LengthNormalized;
        
        var radiusA = circleA.Radius;
        var radiusB = circleB.Radius;
        
        var separation = distance - radiusA - radiusB;
        if (separation > Constants.SpeculativeDistance)
            return Manifold.Empty;
        
        var centerA = pointA + normal * radiusA;
        var centerB = pointB - normal * radiusB;
        var contact = Vector2.Lerp(centerA, centerB, 0.5f);

        var manifoldAnchorA = contact.Rotate(transformA.Rotation);
        var manifoldAnchorB = manifoldAnchorA + (transformA.Position - transformB.Position);
        var manifoldPoint = manifoldAnchorA + transformA.Position;

        return new Manifold(
            normal.Rotate(transformA.Rotation),
            new ManifoldPoint(
                manifoldPoint,
                manifoldAnchorA,
                manifoldAnchorB,
                separation,
                0
            )
        );
    }

    public static Manifold Polygons(ref ShapePolygon polygonA, Transform transformA, ref ShapePolygon polygonB, Transform transformB)
    {
        var origin = polygonA.Vertices[0];
        
        var shiftedTransformA = new Transform(transformA.Position + transformA.Rotate(origin), transformA.Rotation);
        var transform = shiftedTransformA.ToLocalSpaceOf(transformB);
        
        var localA = new ShapePolygon
        {
            Count = polygonA.Count,
            Radius = polygonA.Radius,
        };

        localA.Vertices[0] = Vector2.Zero;
        localA.Normals[0] = polygonA.Normals[0];
        for (var i = 1; i < localA.Count; i++)
        {
            localA.Vertices[i] = polygonA.Vertices[i] - origin;
            localA.Normals[i] = polygonA.Normals[i];
        }

        var localB = new ShapePolygon
        {
            Count = polygonB.Count,
            Radius = polygonB.Radius,
        };
        
        for (var i = 0; i < localB.Count; ++i)
        {
            localB.Vertices[i] = transform.TransformPoint(polygonB.Vertices[i]);
            localB.Normals[i] = transform.Rotate(polygonB.Normals[i]);
        }
        
        var radius = localA.Radius + localB.Radius;
        
        var edgeA = 0;
        var separationA = PhysicsMath.ComputeMaxSeparation(ref edgeA, ref localA, ref localB);
        if (separationA > Constants.SpeculativeDistance + radius)
            return Manifold.Empty;

        var edgeB = 0;
        var separationB = PhysicsMath.ComputeMaxSeparation(ref edgeB, ref localB, ref localA);
        if (separationB > Constants.SpeculativeDistance + radius)
            return Manifold.Empty;

        var flip = separationA < separationB;
        PhysicsMath.ComputeBestSeparatingEdge(ref edgeA,  ref edgeB, ref localA, ref localB, flip);
        
        var manifold = ClipPolygons(ref localA, ref localB, edgeA, edgeB, flip);
        if (manifold.PointCount == 0)
            return Manifold.Empty;
        
        var normal = transformA.Rotate(manifold.Normal);
        var points = new FixedArray2<ManifoldPoint>();
        
        for (var i = 0; i < manifold.PointCount; ++i)
        {
            var oldPoint = manifold.Points[i];

            var anchorA = transformA.Rotate(oldPoint.AnchorA + origin);
            
            points[i] = new ManifoldPoint
            {
                AnchorA = anchorA,
                AnchorB = anchorA + (transformA.Position - transformB.Position),
                Point = anchorA + transformA.Position,
                Separation = oldPoint.Separation,
                Id = oldPoint.Id,
            };
        }

        return new Manifold(normal, 0, points, 2);
    }

    public static Manifold PolygonCircle(ref ShapePolygon polygonA, Transform transformA, ref ShapeCircle circleB, Transform transformB)
    {
        var transform = transformA.ToLocalSpaceOf(transformB);

        var center = transform.TransformPoint(circleB.Center);

        var radiusA = polygonA.Radius;
        var radiusB = circleB.Radius;
        
        var radius = radiusA + radiusB;

        var normalIndex = 0;
        var separation = -float.MaxValue;

        var vertices = polygonA.Vertices;
        var normals = polygonA.Normals;

        var count = polygonA.Count;
        for (var i = 0; i < count; i++)
        {
            var sep = Vector2.Dot(normals[i], center - vertices[i]);
            if (sep <= separation)
                continue;

            separation = sep;
            normalIndex = i;
        }

        if (separation > Constants.SpeculativeDistance + radius)
            return Manifold.Empty;

        var vertexIndex1 = normalIndex;
        var vertexIndex2 = vertexIndex1 + 1 < count ? vertexIndex1 + 1 : 0;

        var vertex1 = vertices[vertexIndex1];
        var vertex2 = vertices[vertexIndex2];

        var u1 = Vector2.Dot(center - vertex1, vertex2 - vertex1);
        if (u1 < 0f && separation > Constants.Epsilon)
        {
            var normal = PhysicsMath.Normalize(center - vertex1);

            separation = Vector2.Dot(center - vertex1, normal);
            if (separation > Constants.SpeculativeDistance + radius)
                return Manifold.Empty;

            var cA = vertex1 + radiusA * normal;
            var cB = center  - radiusB * normal;
      
            var contactPointA = Vector2.Lerp(cA, cB, 0.5f);

            var anchorA = transformA.Rotate(contactPointA);
            var point = new ManifoldPoint
            {
                AnchorA = anchorA,
                AnchorB = anchorA + (transformA.Position - transformB.Position),
                Point = anchorA + transformA.Position,
                Separation = Vector2.Dot(cB - cA, normal),
                Id = 0
            };
            
            var manifold = new Manifold
            {
                Normal = transformA.Rotate(normal),
                Points = new FixedArray2<ManifoldPoint>(point),
                PointCount = 1,
            };
            
            return manifold;
        }
        
        var u2 = Vector2.Dot(center - vertex2, vertex1 - vertex2);
        if (u2 < 0f && separation > Constants.Epsilon)
        {
            var normal = PhysicsMath.Normalize(center - vertex2);

            separation = Vector2.Dot(center - vertex2, normal);
            if (separation > Constants.SpeculativeDistance + radius)
                return Manifold.Empty;

            var cA = vertex2 + radiusA * normal;
            var cB = center  - radiusB * normal;
            
            var contactPointA = Vector2.Lerp(cA, cB, 0.5f);

            var anchorA = transformA.Rotate(contactPointA);
            var point = new ManifoldPoint
            {
                AnchorA = anchorA,
                AnchorB = anchorA + (transformA.Position - transformB.Position),
                Point = anchorA + transformA.Position,
                Separation = Vector2.Dot(cB - cA, normal),
                Id = 0  
            };
            
            var manifold = new Manifold
            {
                Normal = transformA.Rotate(normal),
                Points = new FixedArray2<ManifoldPoint>(point),
                PointCount = 1,
            };

            return manifold;
        }

        {
            var normal = normals[normalIndex];
        
            var cA = center + (radiusA - Vector2.Dot(center - vertex1, normal)) * normal;
            var cB = center - radiusB * normal;

            var contactPointA = Vector2.Lerp(cA, cB, 0.5f);
            
            var anchorA = transformA.Rotate(contactPointA);
            var point = new ManifoldPoint
            {
                AnchorA = anchorA,
                AnchorB = anchorA + (transformA.Position - transformB.Position),
                Point = anchorA + transformA.Position,
                Separation = separation - radius,
                Id = 0
            };
            
            var manifold = new Manifold
            {
                Normal = transformA.Rotate(normal),
                Points = new FixedArray2<ManifoldPoint>(point),
                PointCount = 1,
            };
            
            return manifold;
        }
    }

    private static Manifold ClipPolygons(ref ShapePolygon polygonA, ref ShapePolygon polygonB, int edgeA, int edgeB, bool flip)
    {
        ref var poly1 = ref polygonA;
        ref var poly2 = ref polygonB;

        int i11, i12, i21, i22;

        if (flip)
        {
            poly1 = ref polygonB;
            poly2 = ref polygonA;

            i11 = edgeB;
            i12 = edgeB + 1 < polygonB.Count ? edgeB + 1 : 0;

            i21 = edgeA;
            i22 = edgeA + 1 < polygonA.Count ? edgeA + 1 : 0;
        }
        else
        {
            poly1 = ref polygonA;
            poly2 = ref polygonB;

            i11 = edgeA;
            i12 = edgeA + 1 < polygonA.Count ? edgeA + 1 : 0;

            i21 = edgeB;
            i22 = edgeB + 1 < polygonB.Count ? edgeB + 1 : 0;
        }
        
        var normal = poly1.Normals[i11];

        // Reference edge vertices
        var v11 = poly1.Vertices[i11];
        var v12 = poly1.Vertices[i12];

        // Incident edge vertices
        var v21 = poly2.Vertices[i21];
        var v22 = poly2.Vertices[i22];

        var tangent = Vector2.CrossScalar(1.0f, normal);

        var lower1 = 0.0f;
        var upper1 = Vector2.Dot(v12 - v11, tangent);

        // Incident edge points opposite of tangent due to CCW winding
        var upper2 = Vector2.Dot(v21 - v11, tangent);
        var lower2 = Vector2.Dot(v22 - v11, tangent);

        // Are the segments disjoint?
        if (upper2 < lower1 || upper1 < lower2)
            return Manifold.Empty;

        var vLower = v22;
        if (lower2 < lower1 && upper2 - lower2 > Constants.Epsilon)
            vLower = Vector2.Lerp(v22, v21, (lower1 - lower2) / (upper2 - lower2));
        
        var vUpper= v21;
        if (upper2 > upper1 && upper2 - lower2 > Constants.Epsilon)
            vUpper = Vector2.Lerp(v22, v21, (upper1 - lower2) / (upper2 - lower2));
  

        // todo vLower can be very close to vUpper, reduce to one point?

        var separationLower = Vector2.Dot(vLower - v11, normal);
        var separationUpper = Vector2.Dot(vUpper - v11, normal);

        var r1 = poly1.Radius;
        var r2 = poly2.Radius;

        // Put contact points at midpoint, accounting for radii
        vLower += 0.5f * (r1 - r2 - separationLower) * normal;
        vUpper += 0.5f * (r1 - r2 - separationUpper) * normal;

        var radius = r1 + r2;
        
        if (flip)
        {
            return new Manifold(
                -normal,
                new ManifoldPoint
                {
                    AnchorA = vUpper,
                    Separation = separationUpper - radius,
                    Id = ManifoldPoint.MakeId(i21, i12),
                },
                new ManifoldPoint
                {
                    AnchorA = vLower,
                    Separation = separationLower - radius,
                    Id = ManifoldPoint.MakeId(i22, i11),
                }
            );
        }
        
        return new Manifold(
            normal,
            new ManifoldPoint
            {
                AnchorA = vLower,
                Separation = separationLower - radius,
                Id = ManifoldPoint.MakeId(i11, i22),
            },
            new ManifoldPoint
            {
                AnchorA = vUpper,
                Separation = separationUpper - radius,
                Id = ManifoldPoint.MakeId(i12, i21),
            }
        );
    }
    

}