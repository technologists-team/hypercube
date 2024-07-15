using Hypercube.Shared.EventBus.Handlers;

namespace Hypercube.Shared.EventBus.Broadcast;

public sealed class EventSubscription : IEquatable<EventSubscription>
{
    public RefHandler Handler { get; }
    public object Equality { get; }

    public EventSubscription(RefHandler refHandler, object equalityObj)
    {
        Handler = refHandler;
        Equality = equalityObj;
    }

    public bool Equals(EventSubscription? other)
    {
        return other is not null && Equals(other.Equality, Equality);
    }

    public override bool Equals(object? obj)
    {
        return obj is EventSubscription registration && Equals(registration);
    }

    public override int GetHashCode()
    {
        return Equality.GetHashCode();
    }
}