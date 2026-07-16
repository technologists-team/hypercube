using System.Threading.Channels;
using Hypercube.Utilities.Threads;
using Hypercube.Windowing.Backend.Commands;
using Hypercube.Windowing.Backend.Events;

namespace Hypercube.Windowing.Backend.Processors;

public sealed class WindowingBackendProcessorMultithread : WindowingBackendProcessor
{
    private readonly ThreadBridge<IEvent> _eventBridge;
    private readonly ThreadBridge<ICommand> _commandBridge;
    private readonly Thread _thread;
    
    private bool _running;
    
    public WindowingBackendProcessorMultithread(IBackend backend) : base(backend)
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

        Executor.OnCommand += cmd =>
        {
            if (Thread.CurrentThread == _thread)
            {
                Commander.Execute(cmd);
                return;
            }
            
            _commandBridge.Raise(cmd);
            Backend.PostEmptyEvent(); 
        };

        Listener.OnEvent += ev => _eventBridge.Raise(ev);

        _running = true;
        _thread = new Thread(BackendLoop)
        {
            Name = "Windowing"
        };
        
        _thread.Start();
    }

    private void BackendLoop()
    {
        while (_running)
        {
            Backend.WaitEvents();
            ProcessCommands();
        }
    }

    public override void OnUpdate()
    {
        ProcessEvents();
    }

    protected override void OnDispose()
    {
        _running = false;
 
        _eventBridge.CompleteWrite();
        _commandBridge.CompleteWrite();
        
        Backend.PostEmptyEvent(); 
        
        _thread.Join(); 
        
        Backend.Terminate();
    }

    private void ProcessCommands()
    {
        while (_commandBridge.TryRead(out var cmd))
        {
            Commander.Execute(cmd);
        }
    }

    private void ProcessEvents()
    {
        while (_eventBridge.TryRead(out var ev))
        {
            Raiser.Raise(ev);
        }
    }
}
