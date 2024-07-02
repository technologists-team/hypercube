using Hypercube.Client.Graphics.Windows.Manager;

namespace Hypercube.Client.Input.Handler;

/// <summary>
/// Receives requests from user input, via <see cref="IWindowManager"/>,
/// and submits them for further work via events.
/// </summary>
public interface IInputHandler
{
    event Action<KeyStateArgs>? KeyUp; 
    event Action<KeyStateArgs>? KeyDown; 
    
    // TODO: Create Analyzer to allow access only for IWindowManager implementation
    void SendKeyState(KeyStateArgs args);
}