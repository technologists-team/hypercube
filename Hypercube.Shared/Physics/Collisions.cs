using Hypercube.Math.Boxs;

namespace Hypercube.Shared.Physics;

public static class Collisions
{
    public static bool Intersects(Box2 a, Box2 b)
    {
        return b.Point0.Y <= a.Point1.Y &&
               b.Point1.Y >= a.Point0.Y &&
               b.Point1.X >= a.Point0.X &&
               b.Point0.X <= a.Point1.X;
    }
}