using Hypercube.Windowing.Core.Windows;
using Hypercube.Windowing.Input;

namespace Hypercube.Windowing.Backend.Handlers;

public delegate void WindowInputKeyHandler(WindowHandle window, Key key, KeyState state, KeyModifiers modifiers, int scancode);
