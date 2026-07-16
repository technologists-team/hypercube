using Hypercube.Windowing.Core;
using Hypercube.Windowing.Core.Joystick;

namespace Hypercube.Windowing.Backend.Events;

public readonly record struct EventJoystick(
    JoystickId Joystick,
    ConnectedState State
) : IEvent;
