namespace Hypercube.Core.Ecs.Core.Events;

[EngineInternal]
public sealed class ComponentEventSubscription : IEquatable<ComponentEventSubscription>
{
    public ComponentEventRefHandler Handler { get; }
    public object Equality { get; }

    public ComponentEventSubscription(ComponentEventRefHandler handler, object equality)
    {
        Handler = handler;
        Equality = equality;
    }
    
    public bool Equals(ComponentEventSubscription? other)
    {
        return other is not null && Equals(other.Equality, Equality);
    }

    public override bool Equals(object? obj)
    {
        return obj is ComponentEventSubscription registration && Equals(registration);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Handler, Equality);
    }
}