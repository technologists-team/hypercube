using Hypercube.Mathematics.Matrices;
using Hypercube.Mathematics.Quaternions;
using Hypercube.Mathematics.Vectors;

namespace Hypercube.Core.Viewports;

[PublicAPI]
public class Camera : ICamera
{
    public static Camera Default => new(DefaultSize, DefaultZNear, DefaultZFar, Vector3.One);

    public static readonly Vector2i DefaultSize = new(640, 320);
    public const float DefaultZNear = -1f;
    public const float DefaultZFar = 100f;

    public Matrix4x4 Projection { get; private set; }
    public Matrix4x4 View { get; private set; }

    private Vector2i _size;
    private Vector3 _scale;
    private float _zNear;
    private float _zFar;

    public Vector2i Size
    {
        get => _size;
        set
        {
            _size = value;
            UpdateProjection();
        }
    }

    public Vector3 Position
    {
        get;
        set
        {
            field = value;
            UpdateView();
        }
    }

    public Quaternion Rotation
    {
        get;
        set
        {
            field = value;
            UpdateView();
        }
    } = Quaternion.Identity;

    public Vector3 Scale
    {
        get => _scale;
        set
        {
            _scale = value;
            UpdateView();
        }
    }

    public float ZNear
    {
        get => _zNear;
        set
        {
            _zNear = value;
            UpdateProjection();
        }
    }

    public float ZFar
    {
        get => _zFar;
        set
        {
            _zFar = value;
            UpdateProjection();
        }
    }

    public Camera(Vector2i size, float zNear, float zFar, Vector3 scale)
    {
        _size = size;
        _zNear = zNear;
        _zFar = zFar;
        _scale = scale;

        UpdateProjection();
        UpdateView();
    }

    public Vector3 ScreenToWorld(Vector2i mousePosition)
    {
        var size = Size;
        if (size.X <= 0 || size.Y <= 0)
            return Vector3.Zero;
        
        var ndcX = 2.0f * mousePosition.X / size.X - 1.0f;
        var ndcY = 1.0f - 2.0f * mousePosition.Y / size.Y;
        
        var ndcPosition = new Vector4(ndcX, ndcY, 0.0f, 1.0f);
        
        var viewProjection = View * Projection;
        var inverted = viewProjection.Inverted();
        
        var worldPositionWithW = ndcPosition * inverted;
        return worldPositionWithW.Xyz / worldPositionWithW.W;
    }
    
    private void UpdateProjection()
    {
        Projection = Matrix4x4.CreateOrthographic(Size, ZNear, ZFar);
    }

    private void UpdateView()
    {
        View = Matrix4x4.CreateTransformSRT(-Position, Rotation.Inversed, 1.0f / Scale);
    }
}