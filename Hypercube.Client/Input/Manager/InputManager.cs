using Hypercube.Client.Input.Handler;
using Hypercube.Shared.Dependency;

namespace Hypercube.Client.Input.Manager;

public sealed class InputManager : IInputManager, IPostInject
{
    [Dependency] private readonly IInputHandler _inputHandler = default!;
    
    private Dictionary<Key, KeyState> _keyStates = new();
    
    public void PostInject()
    {
        _inputHandler.KeyUp += OnKeyUp;
        _inputHandler.KeyDown += OnKeyDown;
    }

    private void OnKeyUp(KeyStateChangedArgs changedArgs)
    {
        
    }

    private void OnKeyDown(KeyStateChangedArgs changedArgs)
    {
        
    }
}