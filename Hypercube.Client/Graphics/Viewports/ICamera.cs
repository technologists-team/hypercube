using Hypercube.Shared.Math.Matrix;
using Hypercube.Shared.Math.Vector;

namespace Hypercube.Client.Graphics.Viewports;

public interface ICamera
{
    Matrix4X4 Projection { get; }
    Vector3 Position { get; }
    void SetPosition(Vector3 position);
}