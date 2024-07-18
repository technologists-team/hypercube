using Hypercube.Math;
using Hypercube.Math.Matrices;
using Hypercube.Math.Transforms;
using Hypercube.Math.Vectors;

namespace Hypercube.Client.Graphics.Viewports;

public sealed class Camera2D : ICamera
{
    public Matrix4X4 Projection { get; private set; }
    public Matrix4X4 View { get; private set; }

    public Vector3 Position => _transform.Position;
    public Vector3 Rotation => _transform.Rotation.ToEuler();
    public Vector3 Scale => _transform.Scale;
    public Vector2Int Size { get; private set; }
    
    private readonly float _zFar;
    private readonly float _zNear;

    private Transform3 _transform = new();

    public Camera2D(Vector2Int size, Vector2 position, float zNear, float zFar)
    {
        Size = size;
        _zNear = zNear;
        _zFar = zFar;
        
        SetPosition(new Vector3(position));
        SetRotation(Vector3.UnitZ * HyperMathF.PI);
        SetScale(new Vector3(32, 32, 1));
        
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
        Projection = Matrix4X4.CreateOrthographic(Size, _zNear, _zFar);
        View = _transform.Matrix;
    }
}