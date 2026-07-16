using Hypercube.Windowing.Backend;
using Hypercube.Windowing.Backend.Handlers;
using Hypercube.Windowing.Backend.Processors;
using Hypercube.Windowing.Core.Monitors;
using Hypercube.Windowing.Core.Windows;
using Hypercube.Windowing.Device;
using Hypercube.Windowing.Monitors;
using Hypercube.Windowing.Windows;

namespace Hypercube.Windowing.Core.Device;

public sealed class WindowingDevice : IWindowingDevice
{
    /// <inheritdoc/>
    public event ErrorHandler? OnError;
   
    private readonly Dictionary<WindowHandle, IWindow> _windows = [];
    private readonly Dictionary<MonitorHandle, IMonitor> _monitors = [];
    
    private readonly WindowingBackendProcessor _processor;
    
    private bool _disposed;
   
    public WindowingDevice(in WindowingDeviceSettings settings)
    {
        _processor = BackendFactory.CreateProcessor(settings.Backend, settings.Multithread);
        
        _processor.Raiser.OnError += (message, code) => OnError?.Invoke(message, code);
        _processor.Executor.Initialize();
    }

    /// <inheritdoc/>
    public IWindow CreateWindowSync(WindowCreateSettings settings)
    {
        var handle = _processor.Executor.WindowCreateSync(settings);
        
        var instance = new Window(handle, this, _processor);
      
        _windows[handle] = instance;

        return instance;
    }

    public void WidowRemove(IWindow window)
    {
        if (!_windows.Remove(window.Handle))
            return;
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        if (_disposed)
            return;
        
        _processor.Dispose();
        _disposed = true;
    }
    
    /// <inheritdoc/>
    public void Update()
    {
        _processor.OnUpdate();
    }

    /// <inheritdoc/>
    public nint GetProcAddress(string name)
    {
        return _processor.Backend.GetProcAddress(name);
    }

    /// <inheritdoc/>
    public IWindow GetContext()
    {
        return _windows[_processor.Backend.WindowGetContext()];
    }
}
