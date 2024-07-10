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
    private float Zoom { get; set; } = 1f;
    
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
        var position = (Vector2)Position;
        var box2 = new Box2(position - HalfSize, position + HalfSize);
        var projection = Matrix4X4.CreateOrthographic(box2, _zNear, _zFar);
        var scale  = Matrix4X4.CreateScale(Zoom);

        Projection = projection * scale;
    }
}