using Hypercube.Core.Input.Args;
using WindowHandle = Hypercube.Core.Windowing.Windows.WindowHandle;

namespace Hypercube.Core.Input.Handler;

public sealed partial class InputHandler
{
    private readonly Dictionary<WindowHandle, MouseStateBuffer> _mouse = new();
    private readonly Dictionary<WindowHandle, Vector2i> _mousePosition = new();

    public Vector2i MousePosition => GetMousePosition(Api.Context);
    
    #region Public API

    public void ClearMouseButtonState()
    {
        foreach (var (_, buffer) in _mouse) buffer.ClearFrameState();
    }

    public bool IsMouseButtonHeld(MouseButton button) =>
        IsMouseButtonState(button, KeyState.Held);

    public bool IsMouseButtonPressed(MouseButton button) =>
        IsMouseButtonState(button, KeyState.Pressed);

    public bool IsMouseButtonReleased(MouseButton button) =>
        IsMouseButtonState(button, KeyState.Released);

    public bool IsMouseButtonState(MouseButton button, KeyState state) =>
        IsMouseButtonState(Api.Context, button, state);

    public void SimulateMouseButton(MouseButtonChangedArgs state) =>
        OnMouseButtonUpdate(Api.Context, state);

    public bool IsMouseButtonHeld(WindowHandle window, MouseButton button) =>
        IsMouseButtonState(window, button, KeyState.Held);

    public bool IsMouseButtonPressed(WindowHandle window, MouseButton button) =>
        IsMouseButtonState(window, button, KeyState.Pressed);

    public bool IsMouseButtonReleased(WindowHandle window, MouseButton button) =>
        IsMouseButtonState(window, button, KeyState.Released);

    public bool IsMouseButtonState(WindowHandle window, MouseButton button, KeyState state) =>
        _mouse.TryGetValue(window, out var buffer) && buffer[state].Contains(button);

    public Vector2i GetMousePosition(WindowHandle window)
        => !_mousePosition.TryGetValue(window, out var position) ? Vector2i.Zero : position;

    public void SimulateMouseButton(WindowHandle window, MouseButtonChangedArgs state) =>
        OnMouseButtonUpdate(window, state);

    #endregion

    private MouseStateBuffer GetMouseStateBuffer(WindowHandle window)
    {
        if (_mouse.TryGetValue(window, out var buffer))
            return buffer;
        
        buffer = new MouseStateBuffer();
        _mouse[window] = buffer;

        return buffer;
    }
    
    #region Inner Types
    
    private readonly struct MouseStateBuffer()
    {
        private readonly HashSet<MouseButton> _pressed = [];
        private readonly HashSet<MouseButton> _released = [];
        private readonly HashSet<MouseButton> _held = [];

        public HashSet<MouseButton> this[KeyState state] => state switch
        {
            KeyState.Pressed => _pressed,
            KeyState.Released => _released,
            KeyState.Held => _held,
            _ => throw new ArgumentOutOfRangeException(nameof(state), state, null)
        };

        public void Apply(MouseButtonChangedArgs state)
        {
            switch (state.State)
            {
                case KeyState.Pressed:
                    _held.Add(state.Button);
                    _pressed.Add(state.Button);
                    break;

                case KeyState.Released:
                    _held.Remove(state.Button);
                    _released.Add(state.Button);
                    break;
            }
        }

        public void ClearFrameState()
        {
            _pressed.Clear();
            _released.Clear();
        }
    }
    
    #endregion
}