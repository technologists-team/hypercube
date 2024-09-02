using JetBrains.Annotations;

namespace Hypercube.Input;

[PublicAPI]
public enum KeyState
{
    Released,
    Pressed,
    Held 
}