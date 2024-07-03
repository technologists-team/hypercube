using Hypercube.Shared.Logging;

namespace Hypercube.Client.Input.Handler;

public sealed class InputHandler : IInputHandler
{
    public event Action<KeyStateChangedArgs>? KeyUp;
    public event Action<KeyStateChangedArgs>? KeyDown;
    
    private readonly ILogger _logger = LoggingManager.GetLogger("input_handler");

    public void SendKeyState(KeyStateChangedArgs changedArgs)
    {
#if DEBUG
        // Use only in Debug build,
        // as this check can take quite a lot of performance during input processing
        if (!Enum.IsDefined(typeof(Key), changedArgs.Key))
        {
            _logger.Warning($"Unknown key {changedArgs.Key} handled");
            return;
        }
#endif
        
        if (changedArgs.Pressed)
        {
            KeyUp?.Invoke(changedArgs);
            return;
        }
        
        KeyDown?.Invoke(changedArgs);
    }
}