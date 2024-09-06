using JetBrains.Annotations;

namespace Hypercube.Graphics.Windowing;

[PublicAPI]
public readonly struct WindowCreateResult
{
    public readonly WindowHandle? Registration;
    public readonly string? Error;
    
    public bool Failed => Registration is null;

    public WindowCreateResult(WindowHandle registration)
    {
        Registration = registration;
        Error = null;
    }
    
    public WindowCreateResult(string error)
    {
        Registration = null;
        Error = error;
    }
}