using Hypercube.Client.Input;
using Hypercube.Client.Input.Handler;
using Hypercube.Shared.Dependency;
using Hypercube.Shared.Math.Matrix;
using Hypercube.Shared.Math.Vector;

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
        var speed = 30f;
        var position = camera.Position;
        var rotation = camera.Rotation;
        
        if (_inputHandler.IsKeyDown(Key.W))
            position -= Vector3.UnitY * delta * speed;

        if (_inputHandler.IsKeyDown(Key.S))
            position += Vector3.UnitY * delta * speed; 

        if (_inputHandler.IsKeyDown(Key.A))
            position += Vector3.UnitX * delta * speed;

        if (_inputHandler.IsKeyDown(Key.D))
            position -= Vector3.UnitX * delta * speed;
        
        if (_inputHandler.IsKeyDown(Key.Q))
            rotation -= Vector3.UnitZ * delta;

        if (_inputHandler.IsKeyDown(Key.E))
            rotation += Vector3.UnitZ * delta;
        
        camera.SetPosition(position);
        camera.SetRotation(rotation);
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