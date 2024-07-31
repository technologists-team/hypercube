using Hypercube.Math.Vectors;

namespace Hypercube.Shared.Physics;

public readonly struct Manifold
{
    public readonly IBody BodyA;
    public readonly IBody BodyB;

    public readonly float Depth;
    public readonly Vector2 Normal;

    public readonly Vector2 Contact1;
    public readonly Vector2 Contact2;
    public readonly int ContactCount;

    public Manifold(IBody bodyA, IBody bodyB, float depth, Vector2 normal, Vector2 contact1, Vector2 contact2, int contactCount)
    {
        BodyA = bodyA;
        BodyB = bodyB;
        Depth = depth;
        Normal = normal;
        Contact1 = contact1;
        Contact2 = contact2;
        ContactCount = contactCount;
    }
}