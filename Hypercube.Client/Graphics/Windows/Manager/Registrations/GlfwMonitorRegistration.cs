using Hypercube.Client.Graphics.Monitors;

namespace Hypercube.Client.Graphics.Windows.Manager.Registrations;

public sealed class GlfwMonitorRegistration(int id, IMonitorHandle handle) : MonitorRegistration(handle)
{
    public int Id = id;
}