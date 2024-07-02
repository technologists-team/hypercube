using Hypercube.Client.Utilities;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Hypercube.Client.Graphics.Windows.Manager;

public sealed partial class GlfwWindowManager
{
    private bool GlfwInit()
    {
        if (!GLFW.Init())
        {
            _logger.Fatal($"Failed to initialize, error:\n{GLFWHelper.GetError()}");
            return false;
        }
        
        // Set callback to handle errors
        GLFW.SetErrorCallback(OnErrorHandled);
        
        _initialized = true;
        
        var version = GLFW.GetVersionString();
        _logger.EngineInfo($"Initialize, version: {version}");
        return true;
    }
    
    private void OnErrorHandled(ErrorCode error, string description)
    {
        _logger.Error(GLFWHelper.FormatError(error, description));
    }
}