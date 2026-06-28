using System.Threading.Channels;
using Hypercube.Utilities.Threads;
using Hypercube.Windowing.Core.Backend.Interaction.Commands;
using Hypercube.Windowing.Core.Backend.Interaction.Commands.Sync;
using Hypercube.Windowing.Core.Backend.Interaction.Events;
using Hypercube.Windowing.Core.Windows;

namespace Hypercube.Windowing.Core.Backend.Handler.Realisation;

public sealed class BackendHandlerMultithread : BackendHandler
{
    private readonly ThreadBridge<IEvent> _eventBridge;
    private readonly ThreadBridge<ICommand> _commandBridge;
    private readonly Thread _thread;
    
    private bool _running;
    
    public BackendHandlerMultithread(BackendProxy proxy) : base(proxy)
    {
        _eventBridge = new ThreadBridge<IEvent>(new BoundedChannelOptions(1024)
        {
            FullMode = BoundedChannelFullMode.Wait,
            SingleReader = true,
            SingleWriter = true,
            AllowSynchronousContinuations = true
        });
        
        _commandBridge = new ThreadBridge<ICommand>(new UnboundedChannelOptions
        {
            SingleReader = true,
            SingleWriter = false
        });

        _running = true;
        _thread = new Thread(() =>
        {
            while (_running)
            {
                Proxy.WaitEvents();
                ProcessCommands();
            }
        })
        {
            Name = "Windowing"
        };
        
        _thread.Start();
    }

    public override void OnUpdate()
    {
        ProcessEvents();
    }

    public override void OnTerminate()
    {
        _running = false;
        _eventBridge.CompleteWrite();
        _commandBridge.CompleteWrite();
    }

    protected override void Execute(ICommand cmd)
    {
        _commandBridge.Raise(cmd);
        Proxy.PostEmptyEvent();
    }

    protected override Task ExecuteAsync(ICommand cmd)
    {
        var tcs = new TaskCompletionSource();
        
        _commandBridge.Raise(new SyncCommandVoidWrapper(cmd, tcs));
        Proxy.PostEmptyEvent();
        
        return tcs.Task; 
    }

    protected override WindowHandle Execute(ICommand<WindowHandle> cmd)
    {
        var tcs = new TaskCompletionSource<WindowHandle>();
        
        _commandBridge.Raise(new SyncCommandResultWrapper<WindowHandle>(cmd, tcs));
        Proxy.PostEmptyEvent();
        
        return tcs.Task.GetAwaiter().GetResult();
    }

    protected override Task<WindowHandle> ExecuteAsync(ICommand<WindowHandle> cmd)
    {
        var tcs = new TaskCompletionSource<WindowHandle>();
        
        _commandBridge.Raise(new SyncCommandResultWrapper<WindowHandle>(cmd, tcs));
        Proxy.PostEmptyEvent();
        
        return tcs.Task;
    }

    protected override void Raise(IEvent ev)
    {
        _eventBridge.Raise(ev);
    }

    private void ProcessCommands()
    {
        while (_commandBridge.TryRead(out var raw))
        {
            switch (raw)
            {
                case ISyncCommandResult<WindowHandle> cmd:
                    var result = Proxy.Execute(cmd.InnerCommand);
                    cmd.SetResult(result);
                    break;
                
                case ISyncCommandVoid cmd:
                    Proxy.Execute(raw);
                    cmd.SetResult();
                    break;
                
                default:
                    Proxy.Execute(raw);
                    break;
            }
        }
    }

    private void ProcessEvents()
    {
        while (_eventBridge.TryRead(out var ev))
        {
            Handle(ev);
        }
    }
}