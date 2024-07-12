namespace Hypercube.Shared.EventBus.Events;

public abstract class CancellableEventArgs : EventArgs
{
    public bool Cancelled { get; private set; }

    public void Cancel()
    {
        Cancelled = true;
    }

    public void UnCancel()
    {
        Cancelled = false;
    }
}