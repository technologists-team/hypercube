using Hypercube.Windowing.Types;

namespace Hypercube.Windowing.Core.Backend.Interaction.Events;

public readonly record struct EventError(
    string Message,
    ErrorCode Code
) : IEvent;
