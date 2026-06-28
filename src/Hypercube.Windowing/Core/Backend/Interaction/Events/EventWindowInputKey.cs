using Hypercube.Windowing.Core.Windows;
using Hypercube.Windowing.Input;

namespace Hypercube.Windowing.Core.Backend.Interaction.Events;

public readonly record struct EventWindowInputKey(
    WindowHandle Window,
    Key Key,
    KeyState State,
    KeyModifiers Modifiers,
    int ScanCode
) : IEvent;
