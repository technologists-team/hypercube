using Hypercube.Math.Vectors;

namespace Hypercube.Shared.Physics.Raycasting;

public readonly struct RaycastResult
{
    public readonly Vector2 HitPosition;
    public readonly float Distance;

    public RaycastResult(float distance, Vector2 hitPosition)
    {
        Distance = distance;
        HitPosition = hitPosition;
    }
}