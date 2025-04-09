using Hypercube.Graphics.Windowing.Settings;
using Hypercube.Mathematics.Vectors;

namespace Hypercube.Graphics.Windowing.Api.Realisations.Headless;

public sealed class HeadlessWindowingApi : BaseWindowingApi
{
    public override WindowingApi Type => WindowingApi.Headless;
    protected override string InternalInfo => string.Empty;
    
    public override bool InternalInit()
    {
        return true;
    }

    public override void InternalTerminate()
    {
    }

    public override void InternalPollEvents()
    {
    }

    public override void InternalPostEmptyEvent()
    {
    }

    public override void InternalWaitEvents()
    {
    }

    public override void InternalWaitEventsTimeout(double timeout)
    {
    }

    public override void InternalMakeContextCurrent(nint window)
    {
    }

    public override nint InternalGetCurrentContext()
    {
        return nint.Zero;
    }

    public override nint InternalWindowCreate(WindowCreateSettings settings)
    {
        return nint.Zero;
    }

    public override void InternalWindowDestroy(nint window)
    {
    }

    public override void InternalWindowSetTitle(nint window, string title)
    {
    }

    public override void InternalWindowSetPosition(nint window, Vector2i position)
    {
    }

    public override void InternalWindowSetSize(nint window, Vector2i size)
    {
    }

    public override nint InternalGetProcAddress(string name)
    {
        return nint.Zero;
    }

    public override void InternalSwapBuffers(nint window)
    {
    }

    public override void InternalSwapInterval(int interval)
    {
    }
}