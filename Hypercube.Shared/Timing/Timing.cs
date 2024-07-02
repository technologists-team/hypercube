using System.Diagnostics;

namespace Hypercube.Shared.Timing;

public class Timing : ITiming
{
    public FrameEventArgs FrameEventArgs => new((float)RealFrameTime.TotalSeconds);
    public TimeSpan RealTime => _stopwatch.Elapsed;
    
    public double Fps { get; private set; }
    public TimeSpan RealFrameTime { get; private set; }

    private readonly Stopwatch _stopwatch = new();
    
    private TimeSpan _lastRealTime;
    private double _fps;
    private double _frameTime;
    
    public Timing()
    {
        _stopwatch.Start();
    }

    public void StartFrame()
    {
        var realTime = RealTime;
        RealFrameTime = realTime - _lastRealTime;
        _lastRealTime = realTime;

        _frameTime += RealFrameTime.TotalSeconds;
        _fps++;
        
        if (_frameTime >= 1d)
        {
            Fps = _fps;
            _fps = 0;
            _frameTime -= 1d;
        }
    }
}