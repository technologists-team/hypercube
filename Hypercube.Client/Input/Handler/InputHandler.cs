using Hypercube.Shared.Logging;

namespace Hypercube.Client.Input.Handler;

public sealed class InputHandler : IInputHandler
{
    public event Action<KeyStateArgs>? KeyUp;
    public event Action<KeyStateArgs>? KeyDown;
    
    private readonly ILogger _logger = LoggingManager.GetLogger("input_handler");

    public void SendKeyState(KeyStateArgs args)
    {
#if DEBUG
        // Use only in Debug build,
        // as this check can take quite a lot of performance during input processing
        if (!Enum.IsDefined(typeof(Key), args.Key))
        {
            _logger.Warning($"Unknown key {args.Key} handled");
            return;
        }
#endif
        
        if (args.Pressed)
        {
            KeyUp?.Invoke(args);
            return;
        }
        
        KeyDown?.Invoke(args);
    }
}