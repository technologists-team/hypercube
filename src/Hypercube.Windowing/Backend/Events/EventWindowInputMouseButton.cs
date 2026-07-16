using Hypercube.Windowing.Core.Windows;
using Hypercube.Windowing.Input;

namespace Hypercube.Windowing.Backend.Events;

public readonly record struct EventWindowInputMouseButton(
    WindowHandle Window,
    MouseButton Button,
    KeyState State,
    KeyModifiers Modifiers
) : IEvent;
