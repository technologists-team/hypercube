using Hypercube.Client.Input.Events.Windowing;
using Hypercube.Input;
using Hypercube.Shared.Dependency;
using Hypercube.Shared.EventBus;
using Hypercube.Shared.Logging;

namespace Hypercube.Client.Input.Handler;

public sealed class InputHandler : IInputHandler, IPostInject
{
    [Dependency] private readonly IEventBus _eventBus = default!;
    
    private readonly Dictionary<Key, KeyState> _keys = [];

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
        var state = args.State.State;
        var key = args.State.Key;
        
#if DEBUG
        // Use only in Debug build,
        // as this check can take quite a lot of performance during input processing
        if (!Enum.IsDefined(typeof(Key), key))
        {
            _logger.Warning($"Unknown key {key} handled");
            return;
        }
#endif

        // Legacy shit, maybe will eat many ram and cpu
        // We made many shit because fucking Key rollover: https://en.wikipedia.org/wiki/Key_rollover
        
        
        _logger.Warning($"{key} {state}");
    }

    private void OnMouseButtonHandled(ref WindowingMouseButtonHandledEvent args)
    {
    }
    
    private void OnScrollHandled(ref WindowingScrollHandledEvent args)
    {
    }

    public bool IsKeyState(Key key, KeyState state)
    {
        return _keys.TryGetValue(key, out var keyState) && keyState == state;
    }

    public bool IsKeyHeld(Key key)
    {
        return _keys.TryGetValue(key, out var keyState) && keyState is KeyState.Held or KeyState.Pressed;
    }

    public bool IsKeyPressed(Key key)
    {
        return IsKeyState(key, KeyState.Pressed);
    }

    public bool IsKeyReleased(Key key)
    {
        return IsKeyState(key, KeyState.Pressed);
    }

    public void KeyClear()
    {
        _keys.Clear();
    }
}