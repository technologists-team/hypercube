using Hypercube.Mathematics.Vectors;
using Hypercube.Windowing.Core.Backend.Interaction.Commands;
using Hypercube.Windowing.Core.Windows;
using Hypercube.Windowing.Windows;

namespace Hypercube.Windowing.Core.Backend.Handler;

public abstract partial class BackendHandler
{
    public void Initialize()
    {
        Execute(new CommandInitialize());
    }

    public void Terminate()
    {
        Execute(new CommandTerminate());
        OnTerminate();
    }

    public WindowHandle WindowCreate(WindowCreateSettings settings)
    {
        return Execute(new CommandWindowCreate(settings));
    }

    public void WindowDestroy(WindowHandle handle)
    {
        Execute(new CommandWindowDestroy(handle));
    }

    public void SetIcon(WindowHandle handle, Icon icon)
    {
        Execute(new CommandWindowSetIcon(handle, icon));
    }

    public void SetPosition(WindowHandle handle, Vector2i position)
    {
        Execute(new CommandWindowSetPosition(handle, position));
    }

    public void SetSize(WindowHandle handle, Vector2i size)
    {
        Execute(new CommandWindowSetSize(handle, size));
    }

    public void SetTitle(WindowHandle handle, string title)
    {
        Execute(new CommandWindowSetTitle(handle, title));
    }
}
