using Hypercube.Shared.EventBus.Events;

namespace Hypercube.Shared.EventBus;

/// <summary>
/// EventBus providing interaction between loosely coupled
/// components according to the principle of "event publisher -> event subscriber".
/// </summary>
public interface IEventBus
{
    void Raise<T>(ref T receiver) where T : IEventArgs;
    void Raise<T>(T receiver) where T : IEventArgs;
    void Raise(object receiver);
    void Subscribe<T>(IEventSubscriber subscriber, EventRefHandler<T> refHandler) where T : IEventArgs;
    void Unsubscribe<T>(IEventSubscriber subscriber) where T : IEventArgs;
}