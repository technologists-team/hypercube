using JetBrains.Annotations;

namespace Hypercube.Windowing.Input;

[PublicAPI]
public enum KeyState : byte
{
    Pressed,    
    Held,
    Released
}
