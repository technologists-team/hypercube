using Hypercube.Client.Graphics.Monitors;
using Hypercube.Client.Graphics.Realisation.OpenGL;
using Hypercube.Client.Graphics.Texturing;
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
    WindowCreateResult WindowCreate(ContextInfo? context, WindowCreateSettings settings, WindowRegistration? contextShare);
    void WindowDestroy(WindowRegistration window);
    void WindowSetTitle(WindowRegistration window, string title);
    void WindowSetMonitor(WindowRegistration window, MonitorRegistration monitor);
    void WindowSetMonitor(WindowRegistration window, MonitorRegistration monitor, Vector2Int vector2Int);
    void WindowRequestAttention(WindowRegistration window);
    void WindowSetOpacity(WindowRegistration window, float opacity);
    void WindowSetVisible(WindowRegistration window, bool visible);
    void WindowSetSize(WindowRegistration window, Vector2Int size);
    void WindowSetPosition(WindowRegistration window, Vector2Int position);
    void WindowSwapBuffers(WindowRegistration window);
    void WindowSetIcons(WindowRegistration window, List<ITexture> images);
    
    void MakeContextCurrent(WindowRegistration? window);
    IEnumerable<ITexture> LoadWindowIcons(ITextureManager textureManager, IResourceManager resourceManager, ResourcePath resPath);
    
    nint GetProcAddress(string procName);
}