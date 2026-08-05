namespace Hypercube.Graphics.Backend;

public interface IBackendFence : IDisposable
{
    /// <summary>
    /// CPU: Clear the lock before using the slot.
    /// </summary>
    void Reset();
        
    /// <summary>
    /// CPU: We're waiting for the GPU to pass through this fence
    /// </summary>
    void Wait();
        
    /// <summary>
    /// GPU: Note that the GPU must insert a barrier after the current commands
    /// </summary>
    void Signal();
}
