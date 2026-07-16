using Hypercube.Windowing.Core;
using Hypercube.Windowing.Core.Monitors;

namespace Hypercube.Windowing.Backend.Events;

public readonly record struct EventMonitor(
    MonitorHandle Monitor,
    ConnectedState State
) : IEvent;
