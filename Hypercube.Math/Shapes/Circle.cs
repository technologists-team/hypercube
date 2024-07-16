using Hypercube.Math.Vectors;

namespace Hypercube.Math.Shapes;

public readonly struct Circle(Vector2 position, float radius)
{
    public readonly Vector2 Position = position;
    public readonly float Radius = radius;

    public float Area => Radius * Radius * HyperMathF.PI;
}