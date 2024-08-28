using System.Runtime.CompilerServices;
using Hypercube.Graphics.Monitors;
using Hypercube.Mathematics.Vectors;
using GlfwVideoMode = OpenTK.Windowing.GraphicsLibraryFramework.VideoMode;
using Monitor = OpenTK.Windowing.GraphicsLibraryFramework.Monitor;
using VideoMode = Hypercube.Graphics.Monitors.VideoMode;

namespace Hypercube.Client.Graphics.Windows.Realisation.GLFW;

public sealed unsafe partial class GlfwWindowing
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
        
        var registration = new GlfwMonitorRegistration(
            id,
            name,
            currentVideoMode.Width,
            currentVideoMode.Height,
            currentVideoMode.RefreshRate,
            modes,
            monitor
        );
        
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
    
    public sealed class GlfwMonitorRegistration : MonitorHandle
    {
        public readonly Monitor* Pointer;


        public GlfwMonitorRegistration(MonitorId id, string name, Vector2Int size, int refreshRate, VideoMode[] videoModes, Monitor* pointer) : base(id, name, size, refreshRate, videoModes)
        {
            Pointer = pointer;
        }

        public GlfwMonitorRegistration(MonitorId id, string name, int width, int height, int refreshRate, VideoMode[] videoModes, Monitor* pointer) : base(id, name, width, height, refreshRate, videoModes)
        {
            Pointer = pointer;
        }
    }
}