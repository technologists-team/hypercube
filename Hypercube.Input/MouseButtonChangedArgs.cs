namespace Hypercube.Input;

public class MouseButtonChangedArgs
{
    public bool Shift => Modifiers.HasFlag(KeyModifiers.Shift);
    public bool Control => Modifiers.HasFlag(KeyModifiers.Control);
    public bool Alt => Modifiers.HasFlag(KeyModifiers.Alt);
    public bool Super => Modifiers.HasFlag(KeyModifiers.Super);
    public bool CapsLock => Modifiers.HasFlag(KeyModifiers.CapsLock);
    public bool NumLock => Modifiers.HasFlag(KeyModifiers.NumLock);
    
    public readonly MouseButton Button;
    public readonly KeyState State;
    public readonly KeyModifiers Modifiers;

    public MouseButtonChangedArgs(MouseButton button, KeyState state, KeyModifiers modifiers)
    {
        Button = button;
        State = state;
        Modifiers = modifiers;
    }
}