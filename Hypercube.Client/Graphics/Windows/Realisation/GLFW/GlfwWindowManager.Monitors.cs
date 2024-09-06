using System.Runtime.CompilerServices;
using Hypercube.Graphics.Monitors;
using Hypercube.OpenGL.Utilities.Helpers;
using JetBrains.Annotations;
using GlfwVideoMode = OpenTK.Windowing.GraphicsLibraryFramework.VideoMode;
using Monitor = OpenTK.Windowing.GraphicsLibraryFramework.Monitor;
using VideoMode = Hypercube.Graphics.Monitors.VideoMode;

namespace Hypercube.Client.Graphics.Windows.Realisation.GLFW;

public sealed unsafe partial class GlfwWindowing
{
    private readonly Dictionary<int, MonitorHandle> _monitors = new();
    
    private MonitorId _primaryMonitorId;
    private MonitorId _nextMonitorId = MonitorId.Zero;
    
    private void InitMonitors()
    { 
        var monitors = OpenTK.Windowing.GraphicsLibraryFramework.GLFW.GetMonitorsRaw(out var count);
        
        for (var i = 0; i < count; i++)
        {
            ThreadSetupMonitor(monitors[i]);
        }
        
        var primary = OpenTK.Windowing.GraphicsLibraryFramework.GLFW.GetPrimaryMonitor();
        var up = OpenTK.Windowing.GraphicsLibraryFramework.GLFW.GetMonitorUserPointer(primary);
        _primaryMonitorId = (int) up;
        
        _logger.EngineInfo($"Initialize monitors, count: {_monitors.Count}");
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private void ThreadSetupMonitor(Monitor* monitor)
    {
        // Create a new monitor index by simply increasing the counter by 1
        var monitorId = new MonitorId(_nextMonitorId++);
        
        // Set our monitor identifier as a pointer
        // to be able to get information from glfw events without problems
        GLFWHelper.SetMonitorUserPointer(monitor, monitorId);
        
        // Block for obtaining basic monitor information for further registration
        var name = OpenTK.Windowing.GraphicsLibraryFramework.GLFW.GetMonitorName(monitor);
        var videoMode = GLFWHelper.GetVideoMode(monitor);
        var videoModes = GLFWHelper.GetVideoModes(monitor);
        
        var registration = new MonitorHandle(
            (nint) monitor,
            monitorId,
            name,
            videoMode,
            videoModes
        );
        
        _monitors[monitorId] = registration;
        _renderer.AddMonitor(registration);
    }
}