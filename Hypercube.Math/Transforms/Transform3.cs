using Hypercube.Math.Matrices;
using Hypercube.Math.Vectors;

namespace Hypercube.Math.Transforms;

public partial struct Transform3 : ITransform
{
    public Matrix4X4 Matrix { get; private set; }
    
    public Vector3 Position { get; private set; }
    public Quaternion Rotation { get; private set; }
    public Vector3 Scale { get; private set; }

    public Transform3()
    {
        Position = Vector3.Zero;
        Rotation = new Quaternion(Vector3.Zero);
        Scale = Vector3.One;
        UpdateMatrix();
    }
    
    public Transform3(Vector3 position, Quaternion rotation, Vector3 scale)
    {
        Position = position;
        Rotation = rotation;
        Scale = scale;
        UpdateMatrix();
    }

    public Transform3 SetPosition(Vector3 position)
    {
        Position = position;
        UpdateMatrix();
        
        return this;
    }
    
    public Transform3 SetRotation(Vector3 vector3)
    {
        Rotation = new Quaternion(vector3);
        UpdateMatrix();
        
        return this;
    }
    
    public Transform3 SetRotation(Quaternion rotation)
    {
        Rotation = rotation;
        UpdateMatrix();
        
        return this;
    }

    public Transform3 SetScale(Vector3 scale)
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