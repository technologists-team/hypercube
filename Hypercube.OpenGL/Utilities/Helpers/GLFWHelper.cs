using System.Runtime.InteropServices;
using Hypercube.Graphics.Monitors;
using Hypercube.Mathematics.Vectors;
using JetBrains.Annotations;
using OpenTK.Windowing.GraphicsLibraryFramework;
using Monitor = OpenTK.Windowing.GraphicsLibraryFramework.Monitor;
using VideoMode = OpenTK.Windowing.GraphicsLibraryFramework.VideoMode;

namespace Hypercube.OpenGL.Utilities.Helpers;

[PublicAPI]
public static unsafe class GLFWHelper
{
    public static void SetMonitorUserPointer(Monitor* monitor, MonitorId monitorId)
    {
        GLFW.SetMonitorUserPointer(monitor, (void*)(int) monitorId);
    }
    
    public static Hypercube.Graphics.Monitors.VideoMode[] GetVideoModes(Monitor* monitor)
    {
        var videoModesPointer = GLFW.GetVideoModesRaw(monitor, out var modeCount);
        var videoModes = new Hypercube.Graphics.Monitors.VideoMode[modeCount];
           
        for (var i = 0; i < videoModes.Length; i++)
        {
            videoModes[i] = ConvertVideoMode(videoModesPointer[i]);
        }

        return videoModes;
    }

    public static Hypercube.Graphics.Monitors.VideoMode GetVideoMode(Monitor* monitor)
    {
        return ConvertVideoMode(*GLFW.GetVideoMode(monitor));
    }

    public static Hypercube.Graphics.Monitors.VideoMode ConvertVideoMode(VideoMode mode)
    {
        return new Hypercube.Graphics.Monitors.VideoMode
        {
            Width = (ushort) mode.Width,
            Height = (ushort) mode.Height,
            RedBits = (byte) mode.RedBits,
            RefreshRate = (ushort) mode.RefreshRate,
            GreenBits = (byte) mode.GreenBits,
            BlueBits = (byte) mode.BlueBits,
        };
    }
    
    public static void GetFramebufferSize(Window* window, out Vector2i framebufferSize)
    {
        GLFW.GetFramebufferSize(window, out var x, out var y);
        framebufferSize = new Vector2i(x, y);
    }
    
    public static void GetFramebufferSize(nint window, out Vector2i framebufferSize)
    {
        GLFW.GetFramebufferSize((Window*)window, out var x, out var y);
        framebufferSize = new Vector2i(x, y);
    }

    public static void GetWindowSize(Window* window, out Vector2i size)
    {
        GLFW.GetWindowSize(window, out var x, out var y);
        size = new Vector2i(x, y);
    }
    
    public static void GetWindowSize(nint window, out Vector2i size)
    {
        GLFW.GetWindowSize((Window*)window, out var x, out var y);
        size = new Vector2i(x, y);
    }
    
    public static string FormatError(ErrorCode error, string description)
    {
        return $"Handled glfw error {error} ({Enum.GetName(typeof(ErrorCode), error)}), description: {description}";
    }
    
    public static string FormatError(ErrorCode error, char* description)
    {
        return FormatError(error, Marshal.PtrToStringUTF8((nint)description) ?? "");
    }
    
    public static string GetError()
    {
        var error = GLFW.GetError(out var description);
        return FormatError(error, description);
    }
}