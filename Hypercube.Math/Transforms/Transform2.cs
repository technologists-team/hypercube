using Hypercube.Math.Matrix;
using Hypercube.Math.Vector;

namespace Hypercube.Math.Transform;

public partial struct Transform2 : ITransform
{
    public Matrix4X4 Matrix { get; private set; }
    
    public Vector2 Position { get; private set; }
    public Angle Rotation { get; private set; }
    public Vector2 Scale { get; private set; }

    public Transform2()
    {
        Position = Vector2.Zero;
        Rotation = Angle.Zero;
        Scale = Vector2.One;
        
        UpdateMatrix();
    }
    
    public Transform2(Vector2 position, Angle rotation, Vector2 scale)
    {
        Position = position;
        Rotation = rotation;
        Scale = scale;
        
        UpdateMatrix();
    }
    
    public Transform2 SetPosition(Vector2 position)
    {
        Position = position;
        UpdateMatrix();
        
        return this;
    }
    
    public Transform2 SetRotation(Angle rotation)
    {
        Rotation = rotation;
        UpdateMatrix();
        
        return this;
    }
    
    public Transform2 SetScale(Vector2 scale)
    {
        Scale = scale;
        UpdateMatrix();
        
        return this;
    }
    
    private void UpdateMatrix()
    {
        Matrix = Matrix4X4.CreateTransform(this);
    }
}