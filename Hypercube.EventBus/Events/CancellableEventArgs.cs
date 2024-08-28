using JetBrains.Annotations;

namespace Hypercube.EventBus.Events;

[PublicAPI]
public abstract class CancellableEventArgs : EventArgs
{
    public bool Cancelled { get; private set; }

    public void Cancel()
    {
        Cancelled = true;
    }

    public void Uncancel()
    {
        Cancelled = false;
    }
}