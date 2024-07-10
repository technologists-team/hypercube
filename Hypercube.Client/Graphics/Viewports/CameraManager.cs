using Hypercube.Shared.Dependency;
using Hypercube.Shared.EventBus;
using Hypercube.Shared.Runtimes.Loop.Event;

namespace Hypercube.Client.Graphics.Viewports;

public class CameraManager : ICameraManager, IPostInject
{
    [Dependency] private readonly IEventBus _eventBus = default!;
    
    public void PostInject()
    {
        _eventBus.Subscribe<UpdateFrameEvent>(OnUpdate);
    }

    private void OnUpdate(UpdateFrameEvent args)
    {
        
    }
}