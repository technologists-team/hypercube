using Hypercube.Core.Analyzers;

namespace Hypercube.Core.Ecs.Core.Events;

[EngineCore]
public sealed class EventSubscription : IEquatable<EventSubscription>
{
    public EventRefHandler Handler { get; }
    private object Equality { get; }

    public EventSubscription(EventRefHandler refHandler, object equalityObj)
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