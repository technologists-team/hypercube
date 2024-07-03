namespace Hypercube.Client.Graphics.Windows;

public readonly struct WindowCreateResult(WindowRegistration? registration, string? error)
{
    public bool Failed => Registration is null;
    
    public readonly WindowRegistration? Registration = registration;
    public readonly string? Error = error;
}