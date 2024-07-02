namespace Hypercube.Client.Graphics.Monitors;

/// <summary>
/// Base class to implement the monitor registration for the required graphics API.
/// </summary>
public abstract class MonitorRegistration(IMonitorHandle handle)
{
    public IMonitorHandle Handle = handle;
}