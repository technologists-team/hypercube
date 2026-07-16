using Hypercube.Windowing.Core;
using Hypercube.Windowing.Core.Monitors;

namespace Hypercube.Windowing.Backend.Handlers;

public delegate void MonitorStateHandler(MonitorHandle monitor, ConnectedState state);
