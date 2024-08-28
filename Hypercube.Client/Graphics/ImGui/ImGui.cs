using Hypercube.Client.Graphics.Events;
using Hypercube.Client.Graphics.ImGui.Events;
using Hypercube.Client.Graphics.Rendering;
using Hypercube.Client.Input.Events;
using Hypercube.Dependencies;
using Hypercube.EventBus;
using Hypercube.ImGui;
using Hypercube.Input;
using Hypercube.Logging;
using Hypercube.Runtime.Events;
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
        
        _eventBus.Subscribe<InputFrameEvent>(this, OnInputFrame);
        _eventBus.Subscribe<UpdateFrameEvent>(this, OnUpdateFrame);
        _eventBus.Subscribe<RenderAfterDrawingEvent>(this, OnRenderUI);
        
        _eventBus.Subscribe<MouseButtonHandledEvent>(this, OnMouseButton);
        _eventBus.Subscribe<KeyHandledEvent>(this, OnKey);
        _eventBus.Subscribe<MousePositionHandledEvent>(this, OnMousePosition);
        _eventBus.Subscribe<ScrollHandledEvent>(this, OnInputScroll);
        _eventBus.Subscribe<CharHandledEvent>(this, OnChar);
    }

    private void OnGraphicsInitialized(ref GraphicsLibraryInitializedEvent args)
    {
        _controller = ImGuiFactory.Create(_renderer.MainWindow);
        _controller.OnErrorHandled += message => _logger.Error(message);
        
        _controller.Initialize();
    }    
    
    private void OnInputFrame(ref InputFrameEvent args)
    {
        _controller.InputFrame();
    }
    
    private void OnUpdateFrame(ref UpdateFrameEvent args)
    {
        _controller.Update(args.DeltaSeconds);
    }

    private void OnRenderUI(ref RenderAfterDrawingEvent args)
    {
        var ev = new ImGuiRenderEvent(this);
        _eventBus.Raise(ev);
        
        _controller.Render();
    }
    
    private void OnKey(ref KeyHandledEvent args)
    {
        _controller.UpdateKey(args.Key, args.State, args.Modifiers);
    }
    
    private void OnMouseButton(ref MouseButtonHandledEvent args)
    {
        _controller.UpdateMouseButtons(args.Button, args.State, args.Modifiers);
    }
    
    private void OnMousePosition(ref MousePositionHandledEvent args)
    {
        _controller.UpdateMousePosition(args.Position);
    }

    private void OnInputScroll(ref ScrollHandledEvent args)
    {
        _controller.UpdateMouseScroll(args.Offset);
    }
    
    private void OnChar(ref CharHandledEvent args)
    {
        _controller.UpdateInputCharacter(args.Char);
    }
}