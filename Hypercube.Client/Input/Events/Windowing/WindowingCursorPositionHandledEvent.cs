using Hypercube.EventBus.Events;
using Hypercube.Mathematics.Vectors;
using JetBrains.Annotations;

namespace Hypercube.Client.Input.Events.Windowing;

[PublicAPI]
public sealed class WindowingCursorPositionHandledEvent : IEventArgs
{
    public readonly double X;
    public readonly double Y;
    
    public Vector2 Position => new(X, Y);
    
    public WindowingCursorPositionHandledEvent(double x, double y)
    {
        X = x;
        Y = y;
    }
}