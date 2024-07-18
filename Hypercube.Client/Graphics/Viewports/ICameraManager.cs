using Hypercube.Math.Matrixs;
using Hypercube.Math.Vectors;

namespace Hypercube.Client.Graphics.Viewports;

public interface ICameraManager
{
    ICamera? MainCamera { get; }
    
    Matrix4X4 Projection { get; }
    Matrix4X4 View { get; }
    
    
    void SetMainCamera(ICamera camera);
    ICamera CreateCamera2D(Vector2Int size);
    void UpdateInput(ICamera? camera, float delta);
}