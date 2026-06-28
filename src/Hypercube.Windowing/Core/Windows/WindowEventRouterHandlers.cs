using Hypercube.Mathematics.Vectors;
using Hypercube.Windowing.Input;

namespace Hypercube.Windowing.Core.Windows;

public delegate void RouterWindowCloseHandler();

public delegate void RouterWindowSizeHandler(Vector2i size);

public delegate void RouterWindowFramebufferSizeHandler(Vector2i size);

public delegate void RouterWindowPositionHandler(Vector2i position);

public delegate void RouterWindowFocusHandler(bool focused);

public delegate void RouterWindowInputKeyHandler(Key key, KeyState state, KeyModifiers modifiers, int scancode);

public delegate void RouterWindowInputCursorHandler(Vector2d position);

public delegate void RouterWindowInputMouseButtonHandler(MouseButton button, KeyState state, KeyModifiers modifiers);

public delegate void RouterWindowInputScrollHandler(Vector2d position);
