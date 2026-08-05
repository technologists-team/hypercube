using Silk.NET.OpenGL;

namespace Hypercube.Graphics.Backend.Realisation.OpenGl;

public sealed class OpenGlBackendFence : IBackendFence
{
    private const SyncBehaviorFlags Flags = SyncBehaviorFlags.None;
    private const SyncCondition Condition = SyncCondition.SyncGpuCommandsComplete;
    private const ulong Timeout = ulong.MaxValue;

    private readonly OpenGlBackend _backend;
    
    private nint _sync;

    private GL Gl => _backend.Gl;

    public OpenGlBackendFence(OpenGlBackend backend)
    {
        _backend = backend;
    }

    public void Reset()
    {
        if (_sync == nint.Zero)
            return;
        
        Gl.DeleteSync(_sync);
        _sync = nint.Zero;
    }

    public void Wait()
    {
        if (_sync == nint.Zero)
            return;
        
        Gl.WaitSync(_sync, Flags, Timeout);
    }

    public void Signal()
    {
        _sync = Gl.FenceSync(Condition, Flags);
    }

    public void Dispose()
    {
        Reset();
    }
}