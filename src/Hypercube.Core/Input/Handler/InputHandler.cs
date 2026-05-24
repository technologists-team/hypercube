using Hypercube.Core.Execution.LifeCycle;
using Hypercube.Core.Input.Args;
using Hypercube.Core.Input.Manager;
using Hypercube.Core.Windowing.Api;
using Hypercube.Core.Windowing.Manager;
using Hypercube.Mathematics.Vectors;
using Hypercube.Utilities.Debugging.Logger;
using Hypercube.Utilities.Dependencies;
using WindowHandle = Hypercube.Core.Windowing.Windows.WindowHandle;

namespace Hypercube.Core.Input.Handler;

/// <summary>
/// A class that directly listens to window events and processes them to work correctly,
/// pre first level work with push handling, lower level would be <see cref="IWindowingApi"/>
/// which takes data directly from the current window handling API.
///
/// In most cases it is better to work with <see cref="IInputManager"/>,
/// which is an add-on on top of the current implementation.
/// </summary>
[UsedImplicitly]
public sealed partial class InputHandler : IInputHandler, IPostInject
{
    [Dependency] private readonly ILogger _logger = null!;
    
    [Dependency] private readonly IWindowingManager _windowing = null!;
    [Dependency] private readonly IRuntimeLoop _runtimeLoop = null!;

    public event Action<string>? OnChar;
    
    private readonly Dictionary<nint, KeyStateBuffer> _keys = new();

    private IWindowingApi Api => _windowing.Api;


    public void OnPostInject()
    {
        _runtimeLoop.Actions.Add(OnUpdate, EngineUpdatePriority.InputHandler); 
        
        Api.OnWindowKey += OnKeyUpdate;
        Api.OnWindowChar += OnCharUpdate;
        Api.OnWindowMousePosition += OnMousePositionUpdate;
        Api.OnWindowMouseButton += OnMouseButtonUpdate;
    }

    public void Clear()
    {
        ClearKeyState();
        ClearMouseButtonState();
    }

    private void OnUpdate(FrameEventArgs args)
    {
        Clear();
    }

    private void OnKeyUpdate(WindowHandle window, KeyChangedArgs state)
    {
        GetKeySateBuffer(window).Apply(state);
    }

    private void OnCharUpdate(WindowHandle window, uint codePoint)
    {
        OnChar?.Invoke(char.ConvertFromUtf32((int) codePoint));
    }

    private void OnMousePositionUpdate(WindowHandle window, Vector2d position)
    {
        _mousePosition[window] = (Vector2i) position;
    }

    private void OnMouseButtonUpdate(WindowHandle window, MouseButtonChangedArgs state)
    {
        GetMouseStateBuffer(window).Apply(state);
    }
}
