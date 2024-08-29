using Hypercube.Input;
using Hypercube.Mathematics.Vectors;
using ImGuiNET;
using Key = Hypercube.Input.Key;
using MouseButton = Hypercube.Input.MouseButton;

namespace Hypercube.ImGui.Implementations;

public partial class OpenGLImGuiController
{
    private const int MouseButtons = 5; 
    
    public void InputFrame()
    {
    }
    
    public void UpdateMousePosition(Vector2i position)
    {
        _io.MousePos = position;
    }

    public void UpdateKey(Key key, KeyState state, KeyModifiers modifiers)
    {
        _io.AddKeyEvent(TranslateKey(key), state is KeyState.Pressed or KeyState.Held);
        
        if (state is not KeyState.Pressed and not KeyState.Held)
            return;
        
        switch (key)
        {
            case Key.LeftControl or Key.RightControl:
                _io.KeyCtrl = true;
                break;
            
            case Key.LeftAlt or Key.RightAlt:
                _io.KeyAlt = true;
                break;
            
            case Key.LeftShift or Key.RightShift:
                _io.KeyShift = true;
                break;
            
            case Key.LeftSuper or Key.RightSuper:
                _io.KeySuper = true;
                break;
        }
    }
    
    public void UpdateMouseButtons(MouseButton button, KeyState state, KeyModifiers modifiers)
    {
        var index = (int) button;
        if (index >= MouseButtons)
            return;
        
        _io.MouseDown[index] = state is KeyState.Pressed or KeyState.Held;
    }

    public void UpdateMouseScroll(Vector2 offset)
    {
        _io.MouseWheelH = offset.X;
        _io.MouseWheel = offset.Y;
    }

    public void UpdateInputCharacter(char character)
    {
        _io.AddInputCharacter(character);
        
        var key = (Key) character;
        switch (key)
        {
            case Key.LeftControl or Key.RightControl:
                _io.KeyCtrl = true;
                break;
            
            case Key.LeftAlt or Key.RightAlt:
                _io.KeyAlt = true;
                break;
            
            case Key.LeftShift or Key.RightShift:
                _io.KeyShift = true;
                break;
            
            case Key.LeftSuper or Key.RightSuper:
                _io.KeySuper = true;
                break;
        }
    }
    
      public static ImGuiKey TranslateKey(Key key)
      {
          return key switch
          {
              >= Key.Digit0 and <= Key.Digit9 => key - Key.Digit0 + ImGuiKey._0,
              >= Key.A and <= Key.Z => key - Key.A + ImGuiKey.A,
              >= Key.Numpad0 and <= Key.Numpad9 => key - Key.Numpad0 + ImGuiKey.Keypad0,
              >= Key.F1 and <= Key.F24 => key - Key.F1 + ImGuiKey.F24,
              _ => key switch
              {
                  Key.Tab => ImGuiKey.Tab,
                  Key.Left => ImGuiKey.LeftArrow,
                  Key.Right => ImGuiKey.RightArrow,
                  Key.Up => ImGuiKey.UpArrow,
                  Key.Down => ImGuiKey.DownArrow,
                  Key.PageUp => ImGuiKey.PageUp,
                  Key.PageDown => ImGuiKey.PageDown,
                  Key.End => ImGuiKey.End,
                  Key.Insert => ImGuiKey.Insert,
                  Key.Delete => ImGuiKey.Delete,
                  Key.Backspace => ImGuiKey.Backspace,
                  Key.Space => ImGuiKey.Space,
                  Key.Enter => ImGuiKey.Enter,
                  Key.Escape => ImGuiKey.Escape,
                  Key.Apostrophe => ImGuiKey.Apostrophe,
                  Key.Comma => ImGuiKey.Comma,
                  Key.Minus => ImGuiKey.Minus,
                  Key.Period => ImGuiKey.Period,
                  Key.Slash => ImGuiKey.Slash,
                  Key.Semicolon => ImGuiKey.Semicolon,
                  Key.Equal => ImGuiKey.Equal,
                  Key.LeftBracket => ImGuiKey.LeftBracket,
                  Key.Backslash => ImGuiKey.Backslash,
                  Key.RightBracket => ImGuiKey.RightBracket,
                  Key.GraveAccent => ImGuiKey.GraveAccent,
                  Key.CapsLock => ImGuiKey.CapsLock,
                  Key.ScrollLock => ImGuiKey.ScrollLock,
                  Key.NumLock => ImGuiKey.NumLock,
                  Key.PrintScreen => ImGuiKey.PrintScreen,
                  Key.Pause => ImGuiKey.Pause,
                  Key.KeyPadDecimal => ImGuiKey.KeypadDecimal,
                  Key.KeyPadDivide => ImGuiKey.KeypadDivide,
                  Key.KeyPadMultiply => ImGuiKey.KeypadMultiply,
                  Key.KeyPadSubtract => ImGuiKey.KeypadSubtract,
                  Key.KeyPadAdd => ImGuiKey.KeypadAdd,
                  Key.KeyPadEnter => ImGuiKey.KeypadEnter,
                  Key.KeyPadEqual => ImGuiKey.KeypadEqual,
                  Key.LeftShift => ImGuiKey.LeftShift,
                  Key.LeftControl => ImGuiKey.LeftCtrl,
                  Key.LeftAlt => ImGuiKey.LeftAlt,
                  Key.LeftSuper => ImGuiKey.LeftSuper,
                  Key.RightShift => ImGuiKey.RightShift,
                  Key.RightControl => ImGuiKey.RightCtrl,
                  Key.RightAlt => ImGuiKey.RightAlt,
                  Key.RightSuper => ImGuiKey.RightSuper,
                  Key.Menu => ImGuiKey.Menu,
                  _ => ImGuiKey.None
              }
          };
      }
}