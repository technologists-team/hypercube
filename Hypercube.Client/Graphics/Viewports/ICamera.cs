using Hypercube.Math.Matrix;
using Hypercube.Math.Vector;

namespace Hypercube.Client.Graphics.Viewports;

public interface ICamera
{
    Matrix4X4 Projection { get; }
    
    Vector3 Position { get; }
    Vector3 Rotation { get; }
    Vector3 Scale { get; }
    Vector2Int Size { get; }
    
    void SetPosition(Vector3 position);
    void SetRotation(Vector3 rotation);
    void SetScale(Vector3 scale);
}