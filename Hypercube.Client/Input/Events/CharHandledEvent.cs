using Hypercube.EventBus.Events;
using JetBrains.Annotations;

namespace Hypercube.Client.Input.Events;

[PublicAPI]
public sealed class CharHandledEvent : IEventArgs
{
    public readonly uint Code; 
        
    public char Char => (char) Code;
    public string String => Code.ToString();
    
    public CharHandledEvent(uint code)
    {
        Code = code;
    }

    public static implicit operator uint(CharHandledEvent @event)
    {
        return @event.Code;
    }
    
    public static implicit operator char(CharHandledEvent @event)
    {
        return @event.Char;
    }
    
    public static implicit operator string(CharHandledEvent @event)
    {
        return @event.String;
    }
}