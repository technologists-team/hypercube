using Hypercube.Client.Graphics.Monitors;
using Monitor = OpenTK.Windowing.GraphicsLibraryFramework.Monitor;

namespace Hypercube.Client.Graphics.Windows.Manager.Registrations;

public sealed unsafe class GlfwMonitorRegistration(Monitor* monitor, IMonitorHandle handle) : MonitorRegistration(handle)
{
    public Monitor* Pointer;
}