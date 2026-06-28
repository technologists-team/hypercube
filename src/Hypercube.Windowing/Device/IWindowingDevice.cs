using Hypercube.Windowing.Core.Backend.Interaction.Handlers;
using Hypercube.Windowing.Types;
using Hypercube.Windowing.Windows;

namespace Hypercube.Windowing.Device;

public interface IWindowingDevice
{
    event ErrorHandler? OnError;
    
    IWindow CreateWindow(WindowCreateSettings settings);

    void Terminate();
    
    void Update();

    /// <summary>
    /// Returns a function pointer resolved in the current window context.
    /// Supported only when <see cref="WindowCreateSettings.ContextType"/> is set to
    /// <see cref="WindowContextType.InternalDesktop"/> or
    /// <see cref="WindowContextType.InternalEmbedded"/>.
    /// </summary>
    /// <param name="name">The name of the function to resolve.</param>
    /// <returns>A native pointer to the requested function.</returns>
    nint GetProcAddress(string name);
}
