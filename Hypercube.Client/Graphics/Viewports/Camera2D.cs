using Hypercube.Shared.Math.Box;
using Hypercube.Shared.Math.Matrix;
using Hypercube.Shared.Math.Vector;

namespace Hypercube.Client.Graphics.Viewports;

public class Camera2D : ICamera
{
    public Vector3 Position { get; private set; }
    
    private readonly float _zFar;
    private readonly float _zNear;
    private Vector2Int Size { get; set; }
    private float Zoom { get; set; } = 10f;
    
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

    public void SetZoom(float zoom)
    {
        Zoom = zoom;
        UpdateProjection();
    }

    private void UpdateProjection()
    {
        var projection = Matrix4X4.CreateOrthographic(HalfSize, _zNear, _zFar);
        var scale = Matrix4X4.CreateScale(Zoom);
        var translate = Matrix4X4.CreateTranslate(Position);
        
        Projection = projection * scale * translate;
    }
}