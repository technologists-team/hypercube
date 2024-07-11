using Hypercube.Shared.Math.Matrix;
using Hypercube.Shared.Math.Vector;

namespace Hypercube.Client.Graphics.Viewports;

public class Camera2D : ICamera
{
    public Vector3 Position { get; private set; }
    public Vector3 Rotation { get; private set; }
    public Vector3 Scale { get; private set; } = Vector3.One;
    
    private readonly float _zFar;
    private readonly float _zNear;
    private Vector2Int Size { get; set; }

    
    private Vector2 HalfSize => Size / 2f;
    public Matrix4X4 Projection { get; private set; }

    public Camera2D(Vector2Int size, Vector2 position, float zNear, float zFar)
    {
        Size = size;
        Position = new Vector3(position);
        _zNear = zNear;
        _zFar = zFar;
        
        UpdateProjection();
    }

    public void SetSize(Vector2Int size)
    {
        Size = size;
        UpdateProjection();
    }
    
    public void SetPosition(Vector3 position)
    {
        Position = position;
        UpdateProjection();
    }

    public void SetRotation(Vector3 rotation)
    {
        Rotation = Rotation.WithZ(rotation.Z);
        UpdateProjection();
    }
    
    public void SetScale(Vector3 scale)
    {
        Scale = scale;
        UpdateProjection();
    }

    private void UpdateProjection()
    {
        var projection = Matrix4X4.CreateOrthographic(Size, _zNear, _zFar);
        
        var translate = Matrix4X4.CreateTranslation(Position);
        var rotation = Matrix4X4.CreateRotationZ(Rotation.Z);
        var scale = Matrix4X4.CreateScale(Scale);
        
        Projection = projection * translate * rotation * scale;
    }
}