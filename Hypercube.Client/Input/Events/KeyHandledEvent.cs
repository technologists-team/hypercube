using Hypercube.Input;
using Hypercube.Shared.EventBus.Events;

namespace Hypercube.Client.Input.Events;

public sealed class KeyHandledEvent : KeyStateChangedArgs, IEventArgs
{
    public KeyHandledEvent(Key key, KeyState state, KeyModifiers modifiers, int scanCode) : base(key, state, modifiers, scanCode)
    {
    }
}