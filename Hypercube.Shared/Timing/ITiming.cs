namespace Hypercube.Shared.Timing;

public interface ITiming
{
    void StartFrame();
    
    double Fps { get; }
    TimeSpan RealTime { get; }
    TimeSpan RealFrameTime { get; }
}