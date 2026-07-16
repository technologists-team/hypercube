using Hypercube.Windowing.Core;
using Hypercube.Windowing.Core.Joystick;

namespace Hypercube.Windowing.Backend.Handlers;

public delegate void JoystickStateHandler(JoystickId joystick, ConnectedState state);
