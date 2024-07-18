using Hypercube.Math.Shapes;
using Hypercube.Math.Vectors;

namespace Hypercube.Shared.Physics.Shapes;

public sealed class RectangleShape : IShape
{
    public ShapeType Type => ShapeType.Rectangle;

    public float Radius { get; set; } = 0f;
    public Vector2 Size { get; set; }
    public Vector2 Position { get; set; }
    
    public Box2 Box2 => new(Position - Size / 2, Position + Size / 2);

    public RectangleShape()
    {
        Size = Vector2.One / 2f;
        Position = Vector2.Zero;
    }
    
    public RectangleShape(Vector2 size)
    {
        Size = size;
        Position = Vector2.Zero;
    }
    
    public RectangleShape(Vector2 size, Vector2 position)
    {
        Size = size;
        Position = position;
    }
}