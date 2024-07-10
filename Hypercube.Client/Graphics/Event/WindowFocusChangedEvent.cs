using Hypercube.Client.Graphics.Windows;

namespace Hypercube.Client.Graphics.Event;

public readonly record struct WindowFocusChangedEvent(WindowRegistration Registration, bool Focused);