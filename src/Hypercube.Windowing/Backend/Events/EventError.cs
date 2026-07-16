using Hypercube.Windowing.Core;

namespace Hypercube.Windowing.Backend.Events;

public readonly record struct EventError(
    string Message,
    ErrorCode Code
) : IEvent;
