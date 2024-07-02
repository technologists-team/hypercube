namespace Hypercube.Client.Input;

public readonly struct KeyStateArgs(Key key, bool pressed, bool repeat, KeyModifiers modifiers, int scanCode)
{
    public bool Shift => modifiers.HasFlag(KeyModifiers.Shift);
    public bool Control => modifiers.HasFlag(KeyModifiers.Control);
    public bool Alt => modifiers.HasFlag(KeyModifiers.Alt);
    public bool Super => modifiers.HasFlag(KeyModifiers.Super);
    public bool CapsLock => modifiers.HasFlag(KeyModifiers.CapsLock);
    public bool NumLock => modifiers.HasFlag(KeyModifiers.NumLock);
    
    public readonly Key Key = key;
    public readonly bool Pressed = pressed;
    public readonly bool Repeat = repeat;
    public readonly KeyModifiers Modifiers = modifiers;
    public readonly int ScanCode = scanCode;
}