using Hypercube.Input;
using Hypercube.Shared.EventBus.Events;

namespace Hypercube.Client.Input.Events.Windowing;

public sealed class WindowingKeyHandledEvent : KeyStateChangedArgs, IEventArgs
{
    public WindowingKeyHandledEvent(Key key, KeyState state, KeyModifiers modifiers, int scanCode) : base(key, state, modifiers, scanCode)
    {
    }
}