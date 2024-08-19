using JetBrains.Annotations;

namespace Hypercube.Graphics.Windowing;

[PublicAPI]
public readonly struct WindowCreateResult
{
    public bool Failed => Registration is null;
    public readonly WindowHandle? Registration;
    public readonly string? Error;

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