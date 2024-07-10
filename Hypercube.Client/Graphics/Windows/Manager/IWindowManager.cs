using Hypercube.Client.Graphics.Monitors;
using Hypercube.Client.Graphics.OpenGL;
using Hypercube.Client.Graphics.Texturing;
using Hypercube.Shared.Math.Vector;

namespace Hypercube.Client.Graphics.Windows.Manager;

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
    IEnumerable<ITexture> LoadWindowIcon(ITextureManager textureMan, string resPath);
    void SetWindowIcons(WindowRegistration window, List<ITexture> images);

    nint GetProcAddress(string procName);
}