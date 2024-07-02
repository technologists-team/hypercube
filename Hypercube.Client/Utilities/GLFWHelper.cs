using System.Runtime.InteropServices;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Hypercube.Client.Utilities;

public static class GLFWHelper
{
    public static string FormatError(ErrorCode error, string description)
    {
        return $"Handled glfw error {error} ({Enum.GetName(typeof(ErrorCode), error)}), description: {description}";
    }
    
    public static unsafe string FormatError(ErrorCode error, char* description)
    {
        return FormatError(error, Marshal.PtrToStringUTF8((nint)description) ?? "");
    }
    
    public static string GetError()
    {
        var error = GLFW.GetError(out var description);
        return FormatError(error, description);
    }
}