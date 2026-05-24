using Hypercube.Core.Input.Args;
using WindowHandle = Hypercube.Core.Windowing.Windows.WindowHandle;

namespace Hypercube.Core.Input.Handler;

public sealed partial class InputHandler
{
    #region Public API

    public void ClearKeyState()
    {
        foreach (var (_, buffer) in _keys)
        {
            buffer.ClearFrameState();
        }     
    }
    
    public bool IsKeyHeld(Key key) =>
        IsKeyState(key, KeyState.Held);

    public bool IsKeyPressed(Key key) =>
        IsKeyState(key, KeyState.Pressed);

    public bool IsKeyReleased(Key key) =>
        IsKeyState(key, KeyState.Released);

    public bool IsKeyState(Key key, KeyState state) =>
        IsKeyState(Api.Context, key, state);

    public void SimulateKey(KeyChangedArgs state) =>
        SimulateKey(Api.Context, state);

    public bool IsKeyHeld(WindowHandle window, Key key) =>
        IsKeyState(window, key, KeyState.Held);

    public bool IsKeyPressed(WindowHandle window, Key key) =>
        IsKeyState(window, key, KeyState.Pressed);

    public bool IsKeyReleased(WindowHandle window, Key key) =>
        IsKeyState(window, key, KeyState.Released);

    public bool IsKeyState(WindowHandle window, Key key, KeyState state)
    {
        if (!_keys.TryGetValue(window, out var buffer))
            return false;
        
        return key == Key.Any
            ? buffer.HasAny(state)
            : buffer[state].Contains(key);
    }

    public void SimulateKey(WindowHandle window, KeyChangedArgs state) =>
        OnKeyUpdate(window, state);

    #endregion
    
    private KeyStateBuffer GetKeySateBuffer(WindowHandle window)
    {
        if (_keys.TryGetValue(window, out var buffer))
            return buffer;
        
        buffer = new KeyStateBuffer();
        _keys[window] = buffer;

        return buffer;
    }
    
    #region Inner Types

    private readonly struct KeyStateBuffer()
    {
        private readonly HashSet<Key> _released = [];
        private readonly HashSet<Key> _pressed = [];
        private readonly HashSet<Key> _held = [];

        public IReadOnlySet<Key> this[KeyState state] => state switch
        {
            KeyState.Released => _released,
            KeyState.Pressed => _pressed,
            KeyState.Held => _held,
            _ => throw new ArgumentOutOfRangeException(nameof(state), state, null)
        };
        
        public bool HasAny(KeyState state) => state switch
        {
            KeyState.Released => _released.Count > 0,
            KeyState.Pressed => _pressed.Count > 0,
            KeyState.Held => _held.Count > 0,
            _ => throw new ArgumentOutOfRangeException(nameof(state), state, null)
        };

        public void Apply(KeyChangedArgs state)
        {
            switch (state.State)
            {
                // Legacy shit, maybe will eat many ram and cpu
                // We made many shit because fucking Key rollover: https://en.wikipedia.org/wiki/Key_rollover
                case KeyState.Pressed:
                    ApplyPressed(state.Key);
                    break;
            
                case KeyState.Released:
                    ApplyReleased(state.Key);
                    break;
                
                case KeyState.Held:
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void ClearFrameState()
        {
            _pressed.Clear();
            _released.Clear();
        }

        private void ApplyPressed(Key key)
        {
            _held.Add(key);
            _pressed.Add(key);
        }

        private void ApplyReleased(Key key)
        {
            _held.Remove(key);
            _released.Add(key);
        }
    }
    
    #endregion
}
