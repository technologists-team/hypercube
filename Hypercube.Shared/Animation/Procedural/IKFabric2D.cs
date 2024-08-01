using Hypercube.Math.Vectors;

namespace Hypercube.Shared.Animation.Procedural;

public sealed class IKFabric2D
{
    public Vector2 Position { get; private set; }
    public Vector2 Target { get; private set; }
    public bool Fixed { get; private set; }
    public float MaxReach { get; private set; }
    
    private readonly List<Segment2D> _segments;
    
    public IKFabric2D(Vector2 position, int segments, float angle, float segmentLength, bool @fixed = true)
    {
        Position = position;
        Fixed = @fixed;
        
        _segments = new List<Segment2D>
        {
            new(position, angle, segmentLength)
        };

        MaxReach += segmentLength;
        
        for (var i = 1; i < segments; i++)
        {
            AddSegment(angle, segmentLength);
        }
    }

    public bool CanReach(Vector2 target)
    {
        return (Position - target).LengthSquared <= MaxReach * MaxReach;
    }

    public void AddSegment(float angle, float length)
    {
        var previous = _segments[^1];

        MaxReach += length;
        var segment = new Segment2D(Vector2.Zero, angle, length);
        
        _segments.Add(segment);
        
        segment.Follow(previous.Position);
    }

    public void Update()
    {
        for (var i = 0; i < _segments.Count; i++)
        {
            var segment = _segments[i];
            segment.Update();
            
            if (i == 0) {
                segment.Follow(Target);
                continue;
            }

            var previous = _segments[i - 1];
            segment.Follow(previous.Position);
        }

        var last = _segments.Count - 1;
        var lastSegment = _segments[last];

        if (Fixed)
            lastSegment.SetPosition(Position);    
        
        lastSegment.Update();
        
        for (var i = last - 1; i >= 0; i--) {
            var segment = _segments[i];
            var nextSegment = _segments[i + 1];

            segment.SetPosition(nextSegment.TargetPosition);
            segment.Update();
        }
    }

    public void SetPosition(Vector3 position)
    {
        Position = position;
    }

    public void SetTarget(Vector3 target)
    {
        Target = target;
    }

    public void SetFixed(bool value)
    {
        Fixed = value;
    }
}