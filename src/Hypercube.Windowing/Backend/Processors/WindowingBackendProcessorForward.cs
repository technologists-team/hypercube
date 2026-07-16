namespace Hypercube.Windowing.Backend.Processors;

public sealed class WindowingBackendProcessorForward : WindowingBackendProcessor
{
    public WindowingBackendProcessorForward(IBackend backend) : base(backend)
    {
        Executor.OnCommand += Commander.Execute;
        Listener.OnEvent += Raiser.Raise;
    }

    public override void OnUpdate()
    {
        Backend.PollEvents();
    }

    protected override void OnDispose()
    {
        Backend.Terminate();
    }
}
