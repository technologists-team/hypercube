namespace Hypercube.Client.Input;

public readonly struct KeyStateChangedArgs(Key key, bool pressed, bool repeat, KeyModifiers modifiers, int scanCode)
{
    public bool Shift => Modifiers.HasFlag(KeyModifiers.Shift);
    public bool Control => Modifiers.HasFlag(KeyModifiers.Control);
    public bool Alt => Modifiers.HasFlag(KeyModifiers.Alt);
    public bool Super => Modifiers.HasFlag(KeyModifiers.Super);
    public bool CapsLock => Modifiers.HasFlag(KeyModifiers.CapsLock);
    public bool NumLock => Modifiers.HasFlag(KeyModifiers.NumLock);
    
    public readonly Key Key = key;
    public readonly bool Pressed = pressed;
    public readonly bool Repeat = repeat;
    public readonly KeyModifiers Modifiers = modifiers;
    public readonly int ScanCode = scanCode;
}