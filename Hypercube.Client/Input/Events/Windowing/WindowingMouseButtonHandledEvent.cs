using Hypercube.EventBus.Events;
using Hypercube.Input;

namespace Hypercube.Client.Input.Events.Windowing;

public class WindowingMouseButtonHandledEvent : MouseButtonChangedArgs, IEventArgs
{
    public WindowingMouseButtonHandledEvent(MouseButton button, KeyState state, KeyModifiers modifiers) : base(button, state, modifiers)
    {
    }
}