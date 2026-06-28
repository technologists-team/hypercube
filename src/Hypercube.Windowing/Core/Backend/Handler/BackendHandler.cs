using Hypercube.Windowing.Core.Backend.Interaction.Commands;
using Hypercube.Windowing.Core.Backend.Interaction.Events;
using Hypercube.Windowing.Core.Windows;

namespace Hypercube.Windowing.Core.Backend.Handler;

public abstract partial class BackendHandler : IDisposable
{    
    protected readonly BackendProxy Proxy;

    protected BackendHandler(BackendProxy proxy)
    {
        Proxy = proxy;
        Proxy.OnEvent += Raise;
    }

    public void Dispose()
    {
        Proxy.OnEvent -= Raise;
        GC.SuppressFinalize(this);
    }

    public nint GetProcAddress(string name)
    {
        return Proxy.GetProcAddress(name);
    }

    public abstract void OnUpdate();
    
    public abstract void OnTerminate();
    
    protected abstract void Execute(ICommand cmd);

    protected abstract Task ExecuteAsync(ICommand cmd);

    protected abstract WindowHandle Execute(ICommand<WindowHandle> cmd);

    protected abstract Task<WindowHandle> ExecuteAsync(ICommand<WindowHandle> cmd);

    protected abstract void Raise(IEvent ev);
}
