﻿using Hypercube.OpenGL.Utilities.Helpers;

namespace Hypercube.Client.Graphics.Windows.Realisation.GLFW;

public sealed partial class GlfwWindowing
{
    private bool GlfwInit()
    {
        if (!OpenTK.Windowing.GraphicsLibraryFramework.GLFW.Init())
        {
            _logger.Fatal($"Failed to initialize, error:\n{GLFWHelper.GetError()}");
            return false;
        }
        
        // Set callback to handle errors
        OpenTK.Windowing.GraphicsLibraryFramework.GLFW.SetErrorCallback(_errorCallback);
        
        _initialized = true;
        
        var version = OpenTK.Windowing.GraphicsLibraryFramework.GLFW.GetVersionString();
        _logger.EngineInfo($"Initialize, version: {version}");
        return true;
    }
}