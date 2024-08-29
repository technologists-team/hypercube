using System.Runtime.InteropServices;
using Hypercube.Mathematics.Vectors;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Hypercube.OpenGL.Utilities.Helpers;

public static unsafe class GLFWHelper
{
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