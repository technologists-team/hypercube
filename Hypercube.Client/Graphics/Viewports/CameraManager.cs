using Hypercube.Client.Input;
using Hypercube.Client.Input.Handler;
using Hypercube.Shared.Dependency;
using Hypercube.Shared.EventBus;
using Hypercube.Shared.Math.Matrix;
using Hypercube.Shared.Math.Vector;
using Hypercube.Shared.Runtimes.Loop.Event;

namespace Hypercube.Client.Graphics.Viewports;

public class CameraManager : ICameraManager, IPostInject
{
    [Dependency] private readonly IEventBus _eventBus = default!;
    [Dependency] private readonly IInputHandler _inputHandler = default!;
    
    public ICamera? MainCamera { get; private set; }

    public Matrix4X4 Projection => MainCamera?.Projection ?? Matrix4X4.Identity;
    
    public void PostInject()
    {
        _eventBus.Subscribe<UpdateFrameEvent>(OnUpdate);
    }

    private void OnUpdate(UpdateFrameEvent args)
    {
        if (MainCamera is null)
            return;
        
        // Debug camera controls
        var position = MainCamera.Position;
        
        if (_inputHandler.IsKeyDown(Key.W))
            position += Vector3.Forward;

        if (_inputHandler.IsKeyDown(Key.S))
            position -= Vector3.Back; 

        if (_inputHandler.IsKeyDown(Key.A))
            position -= Vector3.Forward.Cross(Vector3.Up).Normalized;

        if (_inputHandler.IsKeyDown(Key.D))
            position += Vector3.Forward.Cross(Vector3.Up).Normalized;

        if (_inputHandler.IsKeyDown(Key.Space))
            position += Vector3.Up;

        if (_inputHandler.IsKeyDown(Key.LeftShift))
            position -= Vector3.Up;
        
        MainCamera.SetPosition(position);
    }

    public void SetMainCamera(ICamera camera)
    {
        MainCamera = camera;
    }
    
    public ICamera CreateCamera2D(Vector2Int size)
    {
        return CreateCamera2D(size, Vector2.Zero);
    }
    
    public ICamera CreateCamera2D(Vector2Int size, Vector2 position, float zNear = 0.01f, float zFar = 100f)
    {
        return new Camera2D(size, position, zNear, zFar);
    }
}