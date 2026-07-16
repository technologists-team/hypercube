using Hypercube.Windowing.Core.Windows;
using Hypercube.Windowing.Input;

namespace Hypercube.Windowing.Backend.Handlers;

public delegate void WindowInputMouseButtonHandler(WindowHandle window, MouseButton button, KeyState state, KeyModifiers modifiers);
