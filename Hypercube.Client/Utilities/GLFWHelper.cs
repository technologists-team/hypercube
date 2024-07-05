using System.Runtime.InteropServices;
using Hypercube.Math.Vector;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Hypercube.Client.Utilities;

public static unsafe class GLFWHelper
{
    public static void GetFramebufferSize(Window* window, out Vector2Int framebufferSize)
    {
        GLFW.GetFramebufferSize(window, out var x, out var y);
        framebufferSize = new Vector2Int(x, y);
    }

    public static void GetWindowSize(Window* window, out Vector2Int size)
    {
        GLFW.GetWindowSize(window, out var x, out var y);
        size = new Vector2Int(x, y);
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