using Hypercube.Windowing.Backend;
using Hypercube.Windowing.Core.Backend;
using Hypercube.Windowing.Core.Backend.Handler;
using Hypercube.Windowing.Core.Backend.Interaction.Handlers;
using Hypercube.Windowing.Core.Monitors;
using Hypercube.Windowing.Core.Windows;
using Hypercube.Windowing.Device;
using Hypercube.Windowing.Monitors;
using Hypercube.Windowing.Windows;

namespace Hypercube.Windowing.Core.Device;

public sealed class WindowingDevice : IWindowingDevice
{
    public event ErrorHandler? OnError;
   
    private readonly Dictionary<WindowHandle, IWindow> _windows = [];
    private readonly Dictionary<MonitorHandle, IMonitor> _monitors = [];
   
    private readonly BackendHandler _handler;
   
    public Thread? Thread { get; private set; }  
   
    public WindowingDevice(in WindowingDeviceSettings deviceSettings)
    {
        var backend = BackendFactory.Create(deviceSettings.Backend);
        var proxy = new BackendProxy(backend);
      
        _handler = BackendFactory.Create(proxy, deviceSettings.Multithread);
        _handler.OnError += (message, code) => OnError?.Invoke(message, code);
        _handler.Initialize();
    }

    public IWindow CreateWindow(WindowCreateSettings settings)
    {
        var handle = _handler.WindowCreate(settings);
        var instance = new Window(handle, _handler);
      
        _windows[handle] = instance;

        return instance;
    }

    public void Terminate()
    {
        _handler.Terminate();
    }

    public void Update()
    {
        _handler.OnUpdate();
    }

    public void SwapBuffers()
    {
        throw new NotImplementedException();
    }

    public nint GetProcAddress(string name)
    {
        return _handler.GetProcAddress(name);
    }
}