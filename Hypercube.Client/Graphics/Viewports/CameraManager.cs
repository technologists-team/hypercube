using Hypercube.Client.Input.Handler;
using Hypercube.Dependencies;
using Hypercube.Input;
using Hypercube.Mathematics.Matrices;
using Hypercube.Mathematics.Vectors;

namespace Hypercube.Client.Graphics.Viewports;

public class CameraManager : ICameraManager
{
    [Dependency] private readonly IInputHandler _inputHandler = default!;
    
    public ICamera? MainCamera { get; private set; }
    
    public Matrix4X4 Projection => MainCamera?.Projection ?? Matrix4X4.Identity;
    public Matrix4X4 View => MainCamera?.View ?? Matrix4X4.Identity;
    
    public void UpdateInput(ICamera? camera, float delta)
    {
        if (camera is null)
            return;
        
        // Debug camera controls
        var position = camera.Position;
        var rotation = camera.Rotation;
        var scale = camera.Scale;
        
        var speed = 60f;
        
        if (_inputHandler.IsKeyHeld(Key.W))
            position += Vector3.UnitY * speed * delta;

        if (_inputHandler.IsKeyHeld(Key.S))
            position -= Vector3.UnitY * speed * delta; 

        if (_inputHandler.IsKeyHeld(Key.A))
            position -= Vector3.UnitX * speed * delta;

        if (_inputHandler.IsKeyHeld(Key.D))
            position += Vector3.UnitX * speed * delta;
        
        if (_inputHandler.IsKeyHeld(Key.Q))
            rotation -= Vector3.UnitZ * delta;

        if (_inputHandler.IsKeyHeld(Key.E))
            rotation += Vector3.UnitZ * delta;
        
        if (_inputHandler.IsKeyHeld(Key.T))
            scale -= Vector3.One * delta;

        if (_inputHandler.IsKeyHeld(Key.Y))
            scale += Vector3.One * delta;
        
        camera.SetPosition(position);
        camera.SetRotation(rotation);
        camera.SetScale(scale);
    }

    public void SetMainCamera(ICamera camera)
    {
        MainCamera = camera;
    }
    
    public ICamera CreateCamera2D(Vector2i size)
    {
        return CreateCamera2D(size, Vector2.Zero);
    }
    
    public ICamera CreateCamera2D(Vector2i size, Vector2 position, float zNear = 0.1f, float zFar = 100f)
    {
        return new Camera2D(size, position, zNear, zFar);
    }
}