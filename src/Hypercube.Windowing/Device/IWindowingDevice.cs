using Hypercube.Windowing.Backend.Handlers;
using Hypercube.Windowing.Windows;

namespace Hypercube.Windowing.Device;

/// <summary>
/// Defines the contract for a windowing device that manages windows, monitors, 
/// and the underlying graphics or windowing backend.
/// </summary>
public interface IWindowingDevice : IDisposable
{
    /// <summary>
    /// Occurs when an error is raised by the underlying windowing backend.
    /// </summary>
    event ErrorHandler? OnError;
    
    /// <summary>
    /// Creates a new window with the specified creation settings.
    /// </summary>
    /// <param name="settings">The settings used to configure the new window.</param>
    /// <returns>An <see cref="IWindow"/> instance representing the created window.</returns>
    IWindow CreateWindowSync(WindowCreateSettings settings);
    
    /// <summary>
    /// Processes pending window, input, and system events. 
    /// This method should be called regularly within the main application loop.
    /// </summary>
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

    /// <summary>
    /// Returns the native handle of the current rendering context.
    /// </summary>
    /// <returns>A native pointer representing the current context.</returns>
    IWindow GetContext();
}
