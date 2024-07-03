namespace Hypercube.Shared.EventBus;

public interface IEventBus
{
    void Subscribe<T>(Action<T> callback);
    void Unsubscribe<T>(Action<T> callback);
    void Invoke<T>(T signal);
}