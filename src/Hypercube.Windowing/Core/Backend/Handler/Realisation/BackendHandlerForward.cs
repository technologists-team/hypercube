using Hypercube.Windowing.Core.Backend.Interaction.Commands;
using Hypercube.Windowing.Core.Backend.Interaction.Events;
using Hypercube.Windowing.Core.Windows;

namespace Hypercube.Windowing.Core.Backend.Handler.Realisation;

public sealed class BackendHandlerForward : BackendHandler
{
    public BackendHandlerForward(BackendProxy proxy) : base(proxy)
    {
    }

    public override void OnUpdate()
    {
        Proxy.PollEvents();
    }

    public override void OnTerminate()
    {
    }

    protected override void Execute(ICommand cmd)
    {
        Proxy.Execute(cmd);
    }

    protected override Task ExecuteAsync(ICommand cmd)
    {
        Execute(cmd);
        return Task.CompletedTask;
    }

    protected override WindowHandle Execute(ICommand<WindowHandle> cmd)
    {
        return Proxy.Execute(cmd);
    }

    protected override Task<WindowHandle> ExecuteAsync(ICommand<WindowHandle> cmd)
    {
        return Task.FromResult(Execute(cmd));
    }

    protected override void Raise(IEvent ev)
    {
        Handle(ev);
    }
}
