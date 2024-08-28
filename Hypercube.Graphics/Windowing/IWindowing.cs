using Hypercube.Graphics.Monitors;
using Hypercube.Graphics.Texturing;
using Hypercube.Mathematics.Vectors;
using JetBrains.Annotations;

namespace Hypercube.Graphics.Windowing;

/// <summary>
/// A layer between the API for handling windows, events, input and context and the engine.
/// </summary>
[PublicAPI]
public interface IWindowing : IDisposable
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
    WindowCreateResult WindowCreate(IContextInfo? context, WindowCreateSettings settings, WindowHandle? contextShare);
    void WindowDestroy(WindowHandle window);
    void WindowSetTitle(WindowHandle window, string title);
    void WindowSetMonitor(WindowHandle window, MonitorHandle monitor);
    void WindowSetMonitor(WindowHandle window, MonitorHandle monitor, Vector2Int vector2Int);
    void WindowRequestAttention(WindowHandle window);
    void WindowSetOpacity(WindowHandle window, float opacity);
    void WindowSetVisible(WindowHandle window, bool visible);
    void WindowSetSize(WindowHandle window, Vector2Int size);
    void WindowSetPosition(WindowHandle window, Vector2Int position);
    void WindowSwapBuffers(WindowHandle window);
    void WindowSetIcons(WindowHandle window, List<ITexture> images);
    
    void MakeContextCurrent(WindowHandle? window);
    //IEnumerable<ITexture> LoadWindowIcons(ITextureManager textureManager, IResourceLoader resourceLoader, ResourcePath resPath);
    
    nint GetProcAddress(string procName);
}