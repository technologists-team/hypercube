using Hypercube.Shared.EventBus.Broadcast;

namespace Hypercube.Shared.EventBus.Registrations;

/// <summary>
/// Saves information about a specific event.
/// </summary>
public readonly struct EventRegistration()
{
    private readonly HashSet<EventSubscription> _broadcastRegistrations = new();

    public IReadOnlySet<EventSubscription> BroadcastRegistrations => _broadcastRegistrations;
    
    /// <exception cref="InvalidOperationException"></exception>
    public void Add(EventSubscription registration)
    {
        if (_broadcastRegistrations.Contains(registration))
            throw new InvalidOperationException();

        _broadcastRegistrations.Add(registration);
    }

    /// <exception cref="InvalidOperationException"></exception>
    public void Remove(EventSubscription registration)
    {
        if (!_broadcastRegistrations.Contains(registration))
            throw new InvalidOperationException();

        _broadcastRegistrations.Remove(registration);
    }
}