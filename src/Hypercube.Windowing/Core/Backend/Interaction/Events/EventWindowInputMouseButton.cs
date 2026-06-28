using Hypercube.Windowing.Core.Windows;
using Hypercube.Windowing.Input;

namespace Hypercube.Windowing.Core.Backend.Interaction.Events;

public readonly record struct EventWindowInputMouseButton(
    WindowHandle Window,
    MouseButton Button,
    KeyState State,
    KeyModifiers Modifiers
) : IEvent;
