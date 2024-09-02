using JetBrains.Annotations;

namespace Hypercube.Input;

[PublicAPI, Flags]
public enum KeyModifiers
{
    None = default,
    Shift = 1 << 0,
    Control = 1 << 1,
    Alt = 1 << 2,
    Super = 1 << 3,
    CapsLock = 1 << 4,
    NumLock = 1 << 5,
}