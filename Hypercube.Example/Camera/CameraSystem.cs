using Hypercube.Client.Graphics.Rendering;
using Hypercube.Client.Graphics.Viewports;
using Hypercube.Shared.Dependency;
using Hypercube.Shared.Entities.Realisation;
using Hypercube.Shared.Entities.Realisation.Events;
using Hypercube.Shared.Entities.Realisation.Systems;
using Hypercube.Shared.Entities.Systems.Transform;
using Hypercube.Shared.Runtimes.Loop.Event;

namespace Hypercube.Example.Camera;

public sealed class CameraSystem : EntitySystem
{
    [Dependency] private readonly ICameraManager _cameraManager = default!;
    [Dependency] private readonly IRenderer _renderer = default!;
    
    public override void Initialize()
    {
        base.Initialize();
        
        Subscribe<CameraComponent, ComponentAdded>(OnStartup);
    }

    public override void FrameUpdate(UpdateFrameEvent args)
    {
        base.FrameUpdate(args);

        foreach (var entity in GetEntities<CameraComponent>())
        {
            var transformComponent = GetComponent<TransformComponent>(entity);
            entity.Component.Camera.SetPosition(transformComponent.Transform.Position);
        }
    }

    private void OnStartup(Entity<CameraComponent> entity, ref ComponentAdded args)
    {
        var camera = _cameraManager.CreateCamera2D(_renderer.MainWindow.Size);

        _cameraManager.SetMainCamera(camera);
        entity.Component.Camera = camera;
    }
}