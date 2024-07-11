using Hypercube.Client.Input;
using Hypercube.Client.Input.Handler;
using Hypercube.Shared.Dependency;
using Hypercube.Shared.EventBus;
using Hypercube.Shared.Math.Matrix;
using Hypercube.Shared.Math.Vector;
using Hypercube.Shared.Runtimes.Loop.Event;

namespace Hypercube.Client.Graphics.Viewports;

public class CameraManager : ICameraManager
{
    [Dependency] private readonly IInputHandler _inputHandler = default!;
    
    public ICamera? MainCamera { get; private set; }
    public Matrix4X4 Projection => MainCamera?.Projection ?? Matrix4X4.Identity;
    
    public void UpdateInput(ICamera? camera, float delta)
    {
        if (camera is null)
            return;
        
        // Debug camera controls
        var position = camera.Position;
        
        if (_inputHandler.IsKeyDown(Key.W))
            position += Vector3.Forward * delta;

        if (_inputHandler.IsKeyDown(Key.S))
            position -= Vector3.Forward * delta; 

        if (_inputHandler.IsKeyDown(Key.A))
            position -= Vector3.Right * delta;

        if (_inputHandler.IsKeyDown(Key.D))
            position += Vector3.Right * delta;

        if (_inputHandler.IsKeyDown(Key.Space))
            position += Vector3.Up * delta;

        if (_inputHandler.IsKeyDown(Key.LeftShift))
            position -= Vector3.Up * delta;
        
        camera.SetPosition(position);
    }

    public void SetMainCamera(ICamera camera)
    {
        MainCamera = camera;
    }
    
    public ICamera CreateCamera2D(Vector2Int size)
    {
        return CreateCamera2D(size, Vector2.Zero);
    }
    
    public ICamera CreateCamera2D(Vector2Int size, Vector2 position, float zNear = 0.1f, float zFar = 100f)
    {
        return new Camera2D(size, position, zNear, zFar);
    }
}