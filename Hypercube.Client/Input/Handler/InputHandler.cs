using Hypercube.Client.Input.Events.Windowing;
using Hypercube.Input;
using Hypercube.Shared.Dependency;
using Hypercube.Shared.EventBus;
using Hypercube.Shared.Logging;

namespace Hypercube.Client.Input.Handler;

public sealed class InputHandler : IInputHandler, IPostInject
{
    [Dependency] private readonly IEventBus _eventBus = default!;
    
    private readonly HashSet<Key> _keysRelease = [];
    private readonly HashSet<Key> _keysPressed = [];
    private readonly HashSet<Key> _keysDown = [];
    
    private readonly Logger _logger = LoggingManager.GetLogger("input_handler");

    public void PostInject()
    {
        _eventBus.Subscribe<WindowingCharHandledEvent>(this, OnCharHandled);
        _eventBus.Subscribe<WindowingKeyHandledEvent>(this, OnKeyHandled);
        _eventBus.Subscribe<WindowingMouseButtonHandledEvent>(this, OnMouseButtonHandled);
        _eventBus.Subscribe<WindowingScrollHandledEvent>(this, OnScrollHandled);
    }

    private void OnCharHandled(ref WindowingCharHandledEvent args)
    {
        throw new NotImplementedException();
    }
    
    private void OnKeyHandled(ref WindowingKeyHandledEvent args)
    {
#if DEBUG
        // Use only in Debug build,
        // as this check can take quite a lot of performance during input processing
        if (!Enum.IsDefined(typeof(Key), args.State.Key))
        {
            _logger.Warning($"Unknown key {args.State.Key} handled");
            return;
        }
#endif
        
        if (args.State == KeyState.Press)
        {
            _keysDown.Add(args.State.Key);
            return;
        }
        
        _keysDown.Remove(args.State.Key);
    }

    private void OnMouseButtonHandled(ref WindowingMouseButtonHandledEvent args)
    {
    }
    
    private void OnScrollHandled(ref WindowingScrollHandledEvent args)
    {
    }

    public bool IsKeyDown(Key key)
    {
        return _keysDown.Contains(key);
    }
}