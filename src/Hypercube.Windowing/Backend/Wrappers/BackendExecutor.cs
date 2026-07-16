using Hypercube.Mathematics.Vectors;
using Hypercube.Windowing.Backend.Commands;
using Hypercube.Windowing.Backend.Commands.Window;
using Hypercube.Windowing.Backend.Commands.Window.Icons;
using Hypercube.Windowing.Core.Windows;
using Hypercube.Windowing.Windows;

namespace Hypercube.Windowing.Backend.Wrappers;

public sealed class BackendExecutor
{
    public event Action<ICommand>? OnCommand;

    public void Initialize() 
        => OnCommand?.Invoke(new CommandInitialize());

    public void Terminate() 
        => OnCommand?.Invoke(new CommandTerminate());

    public WindowHandle WindowCreateSync(WindowCreateSettings settings) 
        => ExecuteSync(new CommandWindowCreateSync(settings));
    
    public void WindowDestroy(WindowHandle window) 
        => OnCommand?.Invoke(new CommandWindowDestroy(window));

    public void WindowSetTitle(WindowHandle window, string title) 
        => OnCommand?.Invoke(new CommandWindowSetTitle(window, title));

    public void WindowSetPosition(WindowHandle window, Vector2i position) 
        => OnCommand?.Invoke(new CommandWindowSetPosition(window, position));

    public void WindowSetSize(WindowHandle window, Vector2i size) 
        => OnCommand?.Invoke(new CommandWindowSetSize(window, size));
    
    public void WindowSetIcon(WindowHandle window, Icon icon) 
        => OnCommand?.Invoke(new CommandWindowSetIcon(window, icon));

    public void WindowSetIcons(WindowHandle window, Icon[] icons) 
        => OnCommand?.Invoke(new CommandWindowSetIcons(window, icons));

    public void WindowShow(WindowHandle window) 
        => OnCommand?.Invoke(new CommandWindowShow(window));

    public void WindowHide(WindowHandle window) 
        => OnCommand?.Invoke(new CommandWindowHide(window));

    public void WindowMaximize(WindowHandle window) 
        => OnCommand?.Invoke(new CommandWindowMaximize(window));

    public void WindowMinimize(WindowHandle window) 
        => OnCommand?.Invoke(new CommandWindowMinimize(window));

    public void WindowRestore(WindowHandle window) 
        => OnCommand?.Invoke(new CommandWindowRestore(window));
    
    private T ExecuteSync<T>(ISyncCommand<T> cmd)
    {
        cmd.ResultSource = new TaskCompletionSource<T>(TaskCreationOptions.RunContinuationsAsynchronously);
        OnCommand?.Invoke(cmd);
        return cmd.ResultSource.Task.GetAwaiter().GetResult();
    }
}
