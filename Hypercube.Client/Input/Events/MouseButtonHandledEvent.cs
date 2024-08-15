using Hypercube.EventBus.Events;
using Hypercube.Input;
using JetBrains.Annotations;

namespace Hypercube.Client.Input.Events;

[PublicAPI]
public class MouseButtonHandledEvent : MouseButtonChangedArgs, IEventArgs
{
    public MouseButtonHandledEvent(MouseButton button, KeyState state, KeyModifiers modifiers) : base(button, state, modifiers)
    {
    }
}