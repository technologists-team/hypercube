using Hypercube.Mathematics.Vectors;
using Hypercube.Windowing.Core.Joystick;
using Hypercube.Windowing.Core.Monitors;
using Hypercube.Windowing.Core.Windows;
using Hypercube.Windowing.Input;
using Hypercube.Windowing.Types;

namespace Hypercube.Windowing.Core.Backend.Interaction.Handlers;

public delegate void ErrorHandler(string message, ErrorCode code);

public delegate void MonitorStateHandler(MonitorHandle monitor, ConnectedState state);

public delegate void JoystickStateHandler(JoystickId joystick, ConnectedState state);

public delegate void WindowCloseHandler(WindowHandle window);

public delegate void WindowSizeHandler(WindowHandle window, Vector2i size);

public delegate void WindowFramebufferSizeHandler(WindowHandle window, Vector2i size);

public delegate void WindowPositionHandler(WindowHandle window, Vector2i position);

public delegate void WindowFocusHandler(WindowHandle window, bool focused);

public delegate void WindowInputKeyHandler(WindowHandle window, Key key, KeyState state, KeyModifiers modifiers, int scancode);

public delegate void WindowInputCursorHandler(WindowHandle window, Vector2d position);

public delegate void WindowInputMouseButtonHandler(WindowHandle window, MouseButton button, KeyState state, KeyModifiers modifiers);

public delegate void WindowInputScrollHandler(WindowHandle window, Vector2d position);
