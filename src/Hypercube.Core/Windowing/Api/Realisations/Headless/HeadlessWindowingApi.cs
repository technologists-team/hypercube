using Hypercube.Core.Graphics.Objects.Texturing;
using Hypercube.Core.Windowing.Api.Base;
using Hypercube.Core.Windowing.Monitors;
using Hypercube.Core.Windowing.Windows;

namespace Hypercube.Core.Windowing.Api.Realisations.Headless;

public sealed class HeadlessWindowingApi : BaseWindowingApi
{
    public override WindowingApi Type => WindowingApi.Headless;

    protected override string Info => string.Empty;

    public HeadlessWindowingApi(WindowingApiSettings settings) : base(settings)
    {
    }

    protected override bool InternalInit()
    {
        return true;
    }

    protected override void InternalTerminate()
    {
    }

    protected override void InternalPollEvents()
    {
    }

    protected override void InternalPostEmptyEvent()
    {
    }

    protected override void InternalWaitEvents()
    {
    }

    protected override IReadOnlyList<MonitorInstance> InternalGetMonitorInstances()
    {
        return [];
    }

    protected override void InternalWaitEventsTimeout(double timeout)
    {
    }

    protected override void InternalMakeContextCurrent(WindowHandle window)
    {
    }

    protected override WindowHandle InternalGetCurrentContext()
    {
        return WindowHandle.Zero;
    }

    protected override WindowHandle InternalWindowCreate(WindowCreateSettings settings)
    {
        return WindowHandle.Zero;
    }

    protected override void InternalWindowDestroy(WindowHandle window)
    {
    }

    protected override void InternalWindowSetTitle(WindowHandle window, string title)
    {
    }

    protected override void InternalWindowSetPosition(WindowHandle window, Vector2i position)
    {
    }

    protected override void InternalWindowSetSize(WindowHandle window, Vector2i size)
    {
    }

    protected override void InternalWindowSetFramebufferSize(WindowHandle window, Vector2i size)
    {
    }

    protected override void InternalWindowSetIcon(WindowHandle window, IImage icon)
    {
    }

    protected override void InternalSwapBuffers(WindowHandle window)
    {
    }
    
    public override void SwapInterval(int interval)
    {
    }

    public override nint GetProcAddress(string name)
    {
        return nint.Zero;
    }
}