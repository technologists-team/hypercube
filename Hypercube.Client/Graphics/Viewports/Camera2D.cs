using Hypercube.Shared.Math.Box;
using Hypercube.Shared.Math.Matrix;
using Hypercube.Shared.Math.Vector;

namespace Hypercube.Client.Graphics.Viewports;

public class Camera2D : ICamera
{
    public float ZFar { get; private set; }
    public float ZNear { get; private set; }
    public Vector2 Position { get; private set; }
    public float Zoom { get; private set; }

    public Matrix4X4 Projection;

    public void SetPosition(Vector2 position)
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
        var size = new Box2();
        
        var projection = Matrix4X4.CreateOrthographic(size, ZNear, ZFar);
        var scale  = Matrix4X4.CreateScale(Zoom);

        Projection = projection * scale;
    }
}