using Hypercube.Shared.Math.Matrix;
using Hypercube.Shared.Math.Vector;

namespace Hypercube.Client.Graphics.Viewports;

public interface ICameraManager
{
    ICamera? MainCamera { get; }
    Matrix4X4 Projection { get; }
    void SetMainCamera(ICamera camera);
    ICamera CreateCamera2D(Vector2Int size);
}