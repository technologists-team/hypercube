using Hypercube.Windowing.Core.Joystick;
using Hypercube.Windowing.Types;

namespace Hypercube.Windowing.Core.Backend.Interaction.Events;

public readonly record struct EventJoystick(
    JoystickId Joystick,
    ConnectedState State
) : IEvent;
