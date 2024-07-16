using System.Runtime.CompilerServices;
using Hypercube.Client.Graphics.Monitors;
using GlfwVideoMode = OpenTK.Windowing.GraphicsLibraryFramework.VideoMode;
using Monitor = OpenTK.Windowing.GraphicsLibraryFramework.Monitor;
using VideoMode = Hypercube.Client.Graphics.Monitors.VideoMode;

namespace Hypercube.Client.Graphics.Windows.Realisation.Glfw;

public sealed unsafe partial class GlfwWindowManager
{
    private readonly Dictionary<int, GlfwMonitorRegistration> _monitors = new();
    
    private int _primaryMonitorId;
    private int _nextMonitorId = 1;
    
    private void InitMonitors()
    { 
        var monitors = OpenTK.Windowing.GraphicsLibraryFramework.GLFW.GetMonitorsRaw(out var count);
        
        for (var i = 0; i < count; i++)
        {
            ThreadSetupMonitor(monitors[i]);
        }
        
        var primary = OpenTK.Windowing.GraphicsLibraryFramework.GLFW.GetPrimaryMonitor();
        var up = OpenTK.Windowing.GraphicsLibraryFramework.GLFW.GetMonitorUserPointer(primary);
        _primaryMonitorId = (int)up;
        
        _logger.EngineInfo($"Initialize monitors, count: {_monitors.Count}");
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private void ThreadSetupMonitor(Monitor* monitor)
    {
        var id = _nextMonitorId++;
        
        var name = OpenTK.Windowing.GraphicsLibraryFramework.GLFW.GetMonitorName(monitor);
        var videoMode = OpenTK.Windowing.GraphicsLibraryFramework.GLFW.GetVideoMode(monitor);
        
        var modesPointer = OpenTK.Windowing.GraphicsLibraryFramework.GLFW.GetVideoModesRaw(monitor, out var modeCount);
        var modes = new VideoMode[modeCount];
           
        for (var i = 0; i < modes.Length; i++)
        {
            modes[i] = ConvertVideoMode(modesPointer[i]);
        }
            
        OpenTK.Windowing.GraphicsLibraryFramework.GLFW.SetMonitorUserPointer(monitor, (void*)id);

        var currentVideoMode = ConvertVideoMode(*videoMode);
        
        var handle = new MonitorHandle(
            id,
            name,
            currentVideoMode.Width,
            currentVideoMode.Height,
            currentVideoMode.RefreshRate,
            modes
        );
        
        var registration = new GlfwMonitorRegistration(monitor, handle);;
        _monitors[id] = registration;
        
        _renderer.AddMonitor(registration);
    }

    private static VideoMode ConvertVideoMode(in GlfwVideoMode mode)
    {
        return new()
        {
            Width = (ushort)mode.Width,
            Height = (ushort)mode.Height,
            RedBits = (byte)mode.RedBits,
            RefreshRate = (ushort)mode.RefreshRate,
            GreenBits = (byte)mode.GreenBits,
            BlueBits = (byte)mode.BlueBits,
        };
    }
    
    public sealed class GlfwMonitorRegistration(Monitor* monitor, IMonitorHandle handle) : MonitorRegistration(handle)
    {
        public Monitor* Pointer;
    }
}