namespace Hypercube.Shared.EventBus.Events;

public interface IEventArgs;

public abstract class EventArgs : IEventArgs;

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