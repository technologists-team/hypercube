namespace Hypercube.Input;

public readonly struct KeyStateChangedArgs
{
    public bool Shift => Modifiers.HasFlag(KeyModifiers.Shift);
    public bool Control => Modifiers.HasFlag(KeyModifiers.Control);
    public bool Alt => Modifiers.HasFlag(KeyModifiers.Alt);
    public bool Super => Modifiers.HasFlag(KeyModifiers.Super);
    public bool CapsLock => Modifiers.HasFlag(KeyModifiers.CapsLock);
    public bool NumLock => Modifiers.HasFlag(KeyModifiers.NumLock);
    
    public readonly Key Key;
    public readonly KeyState State;
    public readonly KeyModifiers Modifiers;
    
    public readonly int ScanCode;

    public KeyStateChangedArgs(Key key, KeyState state, KeyModifiers modifiers, int scanCode)
    {
        Key = key;
        State = state;
        Modifiers = modifiers;
        ScanCode = scanCode;
    }

    public static implicit operator KeyState(KeyStateChangedArgs args)
    {
        return args.State;
    }
}