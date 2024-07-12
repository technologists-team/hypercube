using System.Numerics;
using Hypercube.Shared.Math.Matrix;
using Hypercube.Shared.Math.Transform;
using Hypercube.Shared.Math.Vector;
using Quaternion = Hypercube.Shared.Math.Quaternion;
using Vector2 = Hypercube.Shared.Math.Vector.Vector2;
using Vector3 = Hypercube.Shared.Math.Vector.Vector3;

namespace Hypercube.Client.Graphics.Viewports;

public class Camera2D : ICamera
{
    private Transform3 _transform = new();

    public Vector3 Position => _transform.Position;
    public Vector3 Rotation => _transform.Rotation.ToEuler();
    public Vector3 Scale => _transform.Scale;
    
    private readonly float _zFar;
    private readonly float _zNear;
    private Vector2Int Size { get; set; }
    
    private Vector2 HalfSize => Size / 2f;
    public Hypercube.Shared.Math.Matrix.Matrix4X4 Projection { get; private set; }

    public Camera2D(Vector2Int size, Vector2 position, float zNear, float zFar)
    {
        Size = size;
        _zNear = zNear;
        _zFar = zFar;
        
        SetPosition(new Vector3(position));
        
        UpdateProjection();
    }

    public void SetPosition(Vector3 position)
    {
        _transform.SetPosition(position);
        UpdateProjection();
    }
    
    public void SetRotation(Vector3 rotation)
    {
        _transform.SetRotation(Quaternion.FromEuler(0, 0, rotation.Z));
        UpdateProjection();
    }
    
    public void SetScale(Vector3 scale)
    {
        _transform.SetScale(scale);
        UpdateProjection();
    }

    private void UpdateProjection()
    {
        var projection = Matrix4X4.CreateOrthographic(Size, _zNear, _zFar);
        Projection = projection * _transform.Matrix;
    }
}