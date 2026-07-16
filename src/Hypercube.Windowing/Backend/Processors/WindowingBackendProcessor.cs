using Hypercube.Windowing.Backend.Wrappers;

namespace Hypercube.Windowing.Backend.Processors;

public abstract class WindowingBackendProcessor : IDisposable
{
    public readonly IBackend Backend;
    
    public readonly BackendExecutor Executor;
    public readonly BackendRaiser Raiser;

    protected readonly BackendCommander Commander;
    protected readonly BackendListener Listener;

    private bool _disposed;
    
    protected WindowingBackendProcessor(IBackend backend)
    {
        Backend = backend;
                
        Commander = new BackendCommander(backend);
        Listener = new BackendListener(backend);
        
        Executor = new BackendExecutor();
        Raiser = new BackendRaiser();
    }

    public void Dispose()
    {
        if (_disposed)
            return;
        
        Listener.Dispose();
        
        OnDispose();

        _disposed = true;
        GC.SuppressFinalize(this);
    }

    public abstract void OnUpdate();
    
    protected abstract void OnDispose();
}