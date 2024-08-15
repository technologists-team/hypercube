using Hypercube.Client.Graphics.Events;
using Hypercube.Client.Graphics.ImGui.Events;
using Hypercube.Client.Graphics.Rendering;
using Hypercube.Dependencies;
using Hypercube.ImGui;
using Hypercube.Shared.EventBus;
using Hypercube.Shared.Logging;
using Hypercube.Shared.Runtimes.Loop.Event;
using JetBrains.Annotations;

namespace Hypercube.Client.Graphics.ImGui;

[PublicAPI]
public sealed class ImGui : IImGui, IEventSubscriber, IPostInject
{
    [Dependency] private readonly IEventBus _eventBus = default!;
    [Dependency] private readonly IRenderer _renderer = default!;

    private readonly Logger _logger = LoggingManager.GetLogger("im_gui"); 
        
    private IImGuiController _controller = default!;
    
    public void PostInject()
    {
        _eventBus.Subscribe<GraphicsLibraryInitializedEvent>(this, OnGraphicsInitialized);
        _eventBus.Subscribe<UpdateFrameEvent>(this, OnUpdateFrame);
        _eventBus.Subscribe<RenderDrawingEvent>(this, OnRenderDrawing);
    }
    
    private void OnGraphicsInitialized(ref GraphicsLibraryInitializedEvent args)
    {
        _controller = ImGuiFactory.Create(_renderer.MainWindow);
        _controller.OnErrorHandled += message => _logger.Error(message);
        
        _controller.Initialize();
    }
    
    private void OnUpdateFrame(ref UpdateFrameEvent args)
    {
        _controller.Update(args.DeltaSeconds);
    }

    private void OnRenderDrawing(ref RenderDrawingEvent args)
    {
        var ev = new ImGuiRenderEvent(this);
        _eventBus.Raise(ev);
        
        _controller.Render();
    }

    public void Begin(string name)
    {
        _controller.Begin(name);
    }

    public void Text(string label)
    {
        _controller.Text(label);
    }

    public bool Button(string label)
    {
        return _controller.Button(label);
    }

    public void End()
    {
        _controller.End();
    }
}