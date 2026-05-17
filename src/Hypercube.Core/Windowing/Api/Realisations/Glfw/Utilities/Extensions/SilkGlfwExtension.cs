using Hypercube.Core.Windowing.Monitors;
using Hypercube.Core.Windowing.Monitors.Data;
using SilkMonitor = Silk.NET.GLFW.Monitor;
using SilkVideoMode = Silk.NET.GLFW.VideoMode;

namespace Hypercube.Core.Windowing.Api.Realisations.Glfw.Utilities.Extensions;

[EngineInternal]
public static unsafe class SilkGlfwExtension
{
    extension(Silk.NET.GLFW.Glfw glfw)
    {
        public Vector2i GetMonitorPos(SilkMonitor* monitor)
        {
            glfw.GetMonitorPos(monitor, out var x, out var y);
            return new Vector2i(x, y);
        }
        
        public Vector2i GetMonitorPhysicalSize(SilkMonitor* monitor)
        {
            glfw.GetMonitorPhysicalSize(monitor, out var x, out var y);
            return new Vector2i(x, y);
        }
        
        public Vector2 GetMonitorContentScale(SilkMonitor* monitor)
        {
            glfw.GetMonitorContentScale(monitor, out var x, out var y);
            return new Vector2(x, y);
        }
        
        public WorkArea GetMonitorWorkArea(SilkMonitor* monitor)
        {
            glfw.GetMonitorWorkarea(monitor, out var x, out var y, out var width, out var height);
            return new WorkArea(new Vector2i(x, y), new Vector2i(width, height));
        }

        public VideoMode GetMonitorVideoMode(SilkMonitor* monitor)
        {
            return ConvertMode(*glfw.GetVideoMode(monitor));
        }

        public VideoMode[] GetMonitorVideoModes(SilkMonitor* monitor)
        {
            var list = glfw.GetVideoModes(monitor, out var count);
            var result = new VideoMode[count];
            
            for (var i = 0; i < count; i++)
            {
                result[i] = ConvertMode(list[i]);    
            }

            return result;
        }

        public Vector2 GetMonitorDpi(SilkMonitor* monitor)
        {
            return MonitorCreateSettings.CalculateDpi(glfw.GetMonitorVideoMode(monitor), glfw.GetMonitorPhysicalSize(monitor));
        }
    }

    public static VideoMode ConvertMode(SilkVideoMode mode)
    {
        return new VideoMode(
            new Vector2i(mode.Width, mode.Height),
            new ColorBits(mode.RedBits, mode.GreenBits, mode.BlueBits),
            mode.RefreshRate
        );
    }
}
