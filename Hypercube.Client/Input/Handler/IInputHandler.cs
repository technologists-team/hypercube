using Hypercube.Client.Graphics.Windows.Manager;

namespace Hypercube.Client.Input.Handler;

/// <summary>
/// Receives requests from user input, via <see cref="IWindowManager"/>,
/// and submits them for further work via events.
/// </summary>
public interface IInputHandler
{
    event Action<KeyStateChangedArgs>? KeyUp; 
    event Action<KeyStateChangedArgs>? KeyDown; 
    
    // TODO: Create Analyzer to allow access only for IWindowManager implementation
    void SendKeyState(KeyStateChangedArgs changedArgs);
}