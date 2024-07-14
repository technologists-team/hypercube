using Hypercube.Client.Graphics.Monitors;
using Hypercube.Client.Graphics.Realisation.OpenGL;
using Hypercube.Client.Graphics.Texturing;
using Hypercube.Shared.Math.Vector;
using Hypercube.Shared.Resources;
using Hypercube.Shared.Resources.Manager;

namespace Hypercube.Client.Graphics.Windows;

public interface IWindowManager : IDisposable
{
    // Lifecycle
    bool Init();
    void Shutdown();
    
    // Main loop
    void EnterWindowLoop();
    void PollEvents();
    void Terminate();
    
    // Cursor
    
    // Window
    WindowCreateResult WindowCreate(ContextInfo? context, WindowCreateSettings settings, WindowRegistration? contextShare);
    void MakeContextCurrent(WindowRegistration? window);
    void WindowDestroy(WindowRegistration window);
    void WindowSetTitle(WindowRegistration window, string title);
    void WindowSetMonitor(WindowRegistration window, MonitorRegistration monitor, Vector2Int vector2Int);
    void WindowSetMonitor(WindowRegistration window, MonitorRegistration monitor);
    void WindowRequestAttention(WindowRegistration window);
    void WindowSetOpacity(WindowRegistration window, float opacity);
    void WindowSetVisible(WindowRegistration registration, bool visible);
    void WindowSetSize(WindowRegistration registration, Vector2Int size);
    void WindowSwapBuffers(WindowRegistration window);
    IEnumerable<ITexture> LoadWindowIcons(ITextureManager textureManager, IResourceManager resourceManager, ResourcePath resPath);
    void SetWindowIcons(WindowRegistration window, List<ITexture> images);

    nint GetProcAddress(string procName);
}