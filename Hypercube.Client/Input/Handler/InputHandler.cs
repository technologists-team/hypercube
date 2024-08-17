using System.Collections.Frozen;
using Hypercube.Client.Input.Events;
using Hypercube.Client.Input.Events.Windowing;
using Hypercube.Dependencies;
using Hypercube.EventBus;
using Hypercube.Input;
using Hypercube.Mathematics.Vectors;
using Hypercube.Shared.Logging;
using Hypercube.Shared.Runtimes.Loop.Event;
using JetBrains.Annotations;

namespace Hypercube.Client.Input.Handler;

[PublicAPI]
public sealed class InputHandler : IInputHandler, IPostInject
{
    [Dependency] private readonly IEventBus _eventBus = default!;

    /// <summary>
    /// List of all <see cref="KeyState"/> of <see cref="Key"/> for legacy input and raising events.
    /// </summary>
    private readonly FrozenDictionary<KeyState, HashSet<Key>> _keys = new Dictionary<KeyState, HashSet<Key>>
    {
        { KeyState.Held, [] },
        { KeyState.Released, [] },
        { KeyState.Pressed, [] },
    }.ToFrozenDictionary();
    
    /// <summary>
    /// List of all <see cref="KeyState"/> of <see cref="MouseButton"/> for legacy input and raising events.
    /// </summary>
    private readonly FrozenDictionary<KeyState, HashSet<MouseButton>> _mouseButtons = new Dictionary<KeyState, HashSet<MouseButton>>
    {
        { KeyState.Held, [] },
        { KeyState.Released, [] },
        { KeyState.Pressed, [] },
    }.ToFrozenDictionary();
    
    private readonly Logger _logger = LoggingManager.GetLogger("input_handler");
    
    public Vector2 MousePosition { get; private set; }
    
    public void PostInject()
    {
        _eventBus.Subscribe<InputFrameEvent>(this, OnInputFrameUpdate);
        
        _eventBus.Subscribe<WindowingCharHandledEvent>(this, OnCharHandled);
        _eventBus.Subscribe<WindowingCursorPositionHandledEvent>(this, OnCursorPositionHandled);
        _eventBus.Subscribe<WindowingKeyHandledEvent>(this, OnKeyHandled);
        _eventBus.Subscribe<WindowingMouseButtonHandledEvent>(this, OnMouseButtonHandled);
        _eventBus.Subscribe<WindowingScrollHandledEvent>(this, OnScrollHandled);
    }

    private void OnInputFrameUpdate(ref InputFrameEvent args)
    {
        _keys[KeyState.Pressed].Clear();
        _keys[KeyState.Released].Clear();

        foreach (var key in _keys[KeyState.Held])
        {
            // Held event don't support modifiers yet, also scanCode
            _eventBus.Raise(new KeyHandledEvent(key, KeyState.Held, KeyModifiers.None, 0));
        }
        
        _mouseButtons[KeyState.Pressed].Clear();
        _mouseButtons[KeyState.Released].Clear();
        
        foreach (var mouseButton in _mouseButtons[KeyState.Held])
        {
            // Held event don't support modifiers yet
            _eventBus.Raise(new MouseButtonHandledEvent(mouseButton, KeyState.Held, KeyModifiers.None));
        }
    }

    private void OnCharHandled(ref WindowingCharHandledEvent args)
    {
        var ev = new CharHandledEvent(args.Code);
        _eventBus.Raise(ev);
    }
    
    private void OnCursorPositionHandled(ref WindowingCursorPositionHandledEvent args)
    {
        MousePosition = args.Position;
        
        var ev = new MousePositionHandledEvent(args.Position);
        _eventBus.Raise(ev);
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
        
        switch (args.State)
        {
            case KeyState.Held:
                return;
            
            // Legacy shit, maybe will eat many ram and cpu
            // We made many shit because fucking Key rollover: https://en.wikipedia.org/wiki/Key_rollover
            case KeyState.Pressed:
                _keys[KeyState.Held].Add(args.Key);
                _keys[KeyState.Pressed].Add(args.Key);
                break;
            
            case KeyState.Released:
                _keys[KeyState.Held].Remove(args.Key);
                _keys[KeyState.Released].Add(args.Key);
                break;
            
            default:
                throw new ArgumentOutOfRangeException();
        }

        _eventBus.Raise(new KeyHandledEvent(args.Key, args.State, args.Modifiers, args.ScanCode));
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
            case KeyState.Held:
                return;
            
            case KeyState.Pressed:
                _mouseButtons[KeyState.Held].Add(args.Button);
                _mouseButtons[KeyState.Pressed].Add(args.Button);
                break;
            
            case KeyState.Released:
                _mouseButtons[KeyState.Held].Remove(args.Button);
                _mouseButtons[KeyState.Released].Add(args.Button);
                break;
            
            default:
                throw new ArgumentOutOfRangeException();
        }
        
        _eventBus.Raise(new MouseButtonHandledEvent(args.Button, args.State, args.Modifiers));
    }
    
    private void OnScrollHandled(ref WindowingScrollHandledEvent args)
    {
        var ev = new ScrollHandledEvent(args.Offset);
        _eventBus.Raise(ev);
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
        return IsMouseButtonState(button, KeyState.Released);
    }

    public void MouseButtonClear()
    {
        foreach (var (_, mouseButtons) in _mouseButtons)
        {
            mouseButtons.Clear();
        }
    }
}