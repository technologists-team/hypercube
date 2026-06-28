using Hypercube.Windowing.Core.Monitors;
using Hypercube.Windowing.Types;

namespace Hypercube.Windowing.Core.Backend.Interaction.Events;

public readonly record struct EventMonitor(
    MonitorHandle Monitor,
    ConnectedState State
) : IEvent;
