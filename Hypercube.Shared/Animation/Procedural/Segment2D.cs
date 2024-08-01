using Hypercube.Math.Vectors;

namespace Hypercube.Shared.Animation.Procedural;

public sealed class Segment2D
{
    public float Angle { get; private set; }
    public float Length { get; private set; }

    public Vector2 Position { get; private set; }
    public Vector2 TargetPosition { get; private set; }
    
    public Segment2D(Vector2 position, float angle, float length)
    {
        Position = position;
        Angle = angle;
        Length = length;

        Update();
    }

    public void Follow(Vector2 target)
    {
        var direction = target - Position;

        Angle = direction.Angle;
        
        // set magnitude
        direction = direction.Normalized * Length;
        direction *= -1;

        Position = target + direction;
    }

    public void Update()
    {
        var delta = new Vector2(MathF.Cos(Angle), MathF.Sin(Angle)) * Length;
        TargetPosition = Position + delta;
    }

    public void SetPosition(Vector2 position)
    {
        Position = position;
    }
}