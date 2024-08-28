using System.Runtime.CompilerServices;
using Hypercube.Mathematics.Vectors;

namespace Hypercube.Mathematics.Shapes;

public readonly struct Circle(Vector2 position, float radius)
{
    public readonly Vector2 Position = position;
    public readonly float Radius = radius;

    public float Area => Radius * Radius * HyperMathF.PI;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Circle operator +(Circle a, Vector2 b)
    {
        return new Circle(a.Position + b, a.Radius);
    }
}