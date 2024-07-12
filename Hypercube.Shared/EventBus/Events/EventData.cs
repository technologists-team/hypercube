using Hypercube.Shared.EventBus.Events.Broadcast;

namespace Hypercube.Shared.EventBus.Events;

public class EventData
{
    public List<BroadcastRegistration> BroadcastRegistrations;

    public EventData(List<BroadcastRegistration> broadcastRegistrations)
    {
        BroadcastRegistrations = broadcastRegistrations;
    }
}