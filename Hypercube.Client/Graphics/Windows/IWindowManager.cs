using Hypercube.Client.Graphics.Monitors;
using Hypercube.Client.Graphics.Realisation.OpenGL;
using Hypercube.Client.Graphics.Texturing;
using Hypercube.Graphics.Windowing;
using Hypercube.Math.Vectors;
using Hypercube.Shared.Resources;
using Hypercube.Shared.Resources.Manager;

namespace Hypercube.Client.Graphics.Windows;

/// <summary>
/// A layer between the API for handling windows, events, input and context and the engine.
/// </summary>
public interface IWindowManager : IDisposable
{
    /// <summary>
    /// Initializes the implementation in the current thread,
    /// after which it will not be possible to work with it from another thread.
    /// </summary>
    bool Init();
    
    /// <summary>
    /// Completes the work of implementation, as well as exiting the basic life cycles.
    /// Automatically called when disposed.
    /// </summary>
    void Shutdown();
    
    // Main loop
    void EnterWindowLoop();
    void PollEvents();
    void Terminate();
    
    // Window
    WindowCreateResult WindowCreate(ContextInfo? context, WindowCreateSettings settings, WindowHandle? contextShare);
    void WindowDestroy(WindowHandle window);
    void WindowSetTitle(WindowHandle window, string title);
    void WindowSetMonitor(WindowHandle window, MonitorRegistration monitor);
    void WindowSetMonitor(WindowHandle window, MonitorRegistration monitor, Vector2Int vector2Int);
    void WindowRequestAttention(WindowHandle window);
    void WindowSetOpacity(WindowHandle window, float opacity);
    void WindowSetVisible(WindowHandle window, bool visible);
    void WindowSetSize(WindowHandle window, Vector2Int size);
    void WindowSetPosition(WindowHandle window, Vector2Int position);
    void WindowSwapBuffers(WindowHandle window);
    void WindowSetIcons(WindowHandle window, List<ITexture> images);
    
    void MakeContextCurrent(WindowHandle? window);
    IEnumerable<ITexture> LoadWindowIcons(ITextureManager textureManager, IResourceLoader resourceLoader, ResourcePath resPath);
    
    nint GetProcAddress(string procName);
}