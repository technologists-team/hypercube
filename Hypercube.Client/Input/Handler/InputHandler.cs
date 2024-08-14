using System.Collections.Frozen;
using Hypercube.Client.Input.Events.Windowing;
using Hypercube.Input;
using Hypercube.Shared.Dependency;
using Hypercube.Shared.EventBus;
using Hypercube.Shared.Logging;
using Hypercube.Shared.Runtimes.Loop.Event;

namespace Hypercube.Client.Input.Handler;

public sealed class InputHandler : IInputHandler, IPostInject
{
    [Dependency] private readonly IEventBus _eventBus = default!;

    private readonly FrozenDictionary<KeyState, HashSet<Key>> _keys = new Dictionary<KeyState, HashSet<Key>>
    {
        { KeyState.Held, [] },
        { KeyState.Release, [] },
        { KeyState.Pressed, [] },
    }.ToFrozenDictionary();

    private readonly FrozenDictionary<KeyState, HashSet<MouseButton>> _mouseButtons = new Dictionary<KeyState, HashSet<MouseButton>>
    {
        { KeyState.Held, [] },
        { KeyState.Release, [] },
        { KeyState.Pressed, [] },
    }.ToFrozenDictionary();

    
    private readonly Logger _logger = LoggingManager.GetLogger("input_handler");

    public void PostInject()
    {
        _eventBus.Subscribe<InputFrameEvent>(this, OnInputFrameUpdate);
        
        _eventBus.Subscribe<WindowingCharHandledEvent>(this, OnCharHandled);
        _eventBus.Subscribe<WindowingKeyHandledEvent>(this, OnKeyHandled);
        _eventBus.Subscribe<WindowingMouseButtonHandledEvent>(this, OnMouseButtonHandled);
        _eventBus.Subscribe<WindowingScrollHandledEvent>(this, OnScrollHandled);
    }

    private void OnInputFrameUpdate(ref InputFrameEvent args)
    {
        _keys[KeyState.Pressed].Clear();
        _keys[KeyState.Release].Clear();
        _mouseButtons[KeyState.Pressed].Clear();
        _mouseButtons[KeyState.Release].Clear();
    }

    private void OnCharHandled(ref WindowingCharHandledEvent args)
    {
    }
    
    private void OnKeyHandled(ref WindowingKeyHandledEvent args)
    {
#if DEBUG
        // Use only in Debug build,
        // as this check can take quite a lot of performance during input processing
        if (!Enum.IsDefined(typeof(Key), args.Key))
        {
            _logger.Warning($"Unknown {args.Key} handled");
            return;
        }
#endif

        // Legacy shit, maybe will eat many ram and cpu
        // We made many shit because fucking Key rollover: https://en.wikipedia.org/wiki/Key_rollover
        switch (args.State)
        {
            case KeyState.Pressed:
                _keys[KeyState.Held].Add(args.Key);
                _keys[KeyState.Pressed].Add(args.Key);
                break;
            
            case KeyState.Release:
                _keys[KeyState.Held].Remove(args.Key);
                _keys[KeyState.Pressed].Add(args.Key);
                break;
            
            case KeyState.Held:
                break;
            
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void OnMouseButtonHandled(ref WindowingMouseButtonHandledEvent args)
    {
#if DEBUG
        // Use only in Debug build,
        // as this check can take quite a lot of performance during input processing
        if (!Enum.IsDefined(typeof(MouseButton), args.Button))
        {
            _logger.Warning($"Unknown {args.Button} handled");
            return;
        }
#endif
        
        switch (args.State)
        {
            case KeyState.Pressed:
                _mouseButtons[KeyState.Held].Add(args.Button);
                _mouseButtons[KeyState.Pressed].Add(args.Button);
                break;
            
            case KeyState.Release:
                _mouseButtons[KeyState.Held].Remove(args.Button);
                _mouseButtons[KeyState.Pressed].Add(args.Button);
                break;
            
            case KeyState.Held:
                break;
            
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    
    private void OnScrollHandled(ref WindowingScrollHandledEvent args)
    {
    }

    public bool IsKeyState(Key key, KeyState state)
    {
        return _keys[state].Contains(key);
    }

    public bool IsKeyHeld(Key key)
    {
        return IsKeyState(key, KeyState.Held);
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
        foreach (var (_, key) in _keys)
        {
            key.Clear();
        }
    }

    public bool IsMouseButtonState(MouseButton button, KeyState state)
    {
        return _mouseButtons[state].Contains(button);
    }

    public bool IsMouseButtonHeld(MouseButton button)
    {
        return IsMouseButtonState(button, KeyState.Held);
    }

    public bool IsMouseButtonPressed(MouseButton button)
    {
        return IsMouseButtonState(button, KeyState.Pressed);
    }

    public bool IsMouseButtonReleased(MouseButton button)
    {
        return IsMouseButtonState(button, KeyState.Release);
    }

    public void MouseButtonClear()
    {
        foreach (var (_, mouseButtons) in _mouseButtons)
        {
            mouseButtons.Clear();
        }
    }
}