using Hypercube.Shared.EventBus.Broadcast;

namespace Hypercube.Shared.EventBus;

/// <summary>
/// Saves information about a specific event.
/// </summary>
public readonly struct EventRegistration()
{
    private readonly HashSet<BroadcastRegistration> _broadcastRegistrations = new();

    public IReadOnlySet<BroadcastRegistration> BroadcastRegistrations => _broadcastRegistrations;
    
    /// <exception cref="InvalidOperationException"></exception>
    public void Add(BroadcastRegistration registration)
    {
        if (_broadcastRegistrations.Contains(registration))
            throw new InvalidOperationException();

        _broadcastRegistrations.Add(registration);
    }

    /// <exception cref="InvalidOperationException"></exception>
    public void Remove(BroadcastRegistration registration)
    {
        if (!_broadcastRegistrations.Contains(registration))
            throw new InvalidOperationException();

        _broadcastRegistrations.Remove(registration);
    }
}