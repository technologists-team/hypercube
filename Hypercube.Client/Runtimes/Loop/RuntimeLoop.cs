using Hypercube.Shared.Dependency;
using Hypercube.Shared.Timing;

namespace Hypercube.Client.Runtimes.Loop;

public class RuntimeLoop
{
    [Dependency] private readonly ITiming _timing = default!;
    
    public event EventHandler<FrameEventArgs>? Input;
    public event EventHandler<FrameEventArgs>? Tick;
    public event EventHandler<FrameEventArgs>? Update;
    public event EventHandler<FrameEventArgs>? Render;
    
    public bool Running { get; private set; }

    public void Run()
    {
        Running = true;
        while (Running)
        {
            _timing.StartFrame();
            var realFrameEvent = _timing.FrameEventArgs; 
            
            Input?.Invoke(this, realFrameEvent);
            Tick?.Invoke(this, realFrameEvent);
            Update?.Invoke(this, realFrameEvent);
            Render?.Invoke(this, realFrameEvent);
        }
    }

    public void Shutdown()
    {
        Running = false;
    }
}