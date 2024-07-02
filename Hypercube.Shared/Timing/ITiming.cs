namespace Hypercube.Shared.Timing;

public interface ITiming
{
    void StartFrame();
    
    FrameEventArgs FrameEventArgs { get; }
    double Fps { get; }
    TimeSpan RealTime { get; }
    TimeSpan RealFrameTime { get; }
}