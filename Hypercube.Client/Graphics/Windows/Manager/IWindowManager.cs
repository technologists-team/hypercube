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
    void WindowSetTitle();
    void WindowSetMonitor();
    void WindowRequestAttention();
    void WindowSetVisible();
    void WindowSwapBuffers(WindowRegistration window);

    nint GetProcAddress(string procName);
}