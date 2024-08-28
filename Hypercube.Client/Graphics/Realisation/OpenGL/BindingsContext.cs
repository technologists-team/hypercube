using Hypercube.Graphics.Windowing;
using OpenToolkit;

namespace Hypercube.Client.Graphics.Realisation.OpenGL;

public sealed class BindingsContext(IWindowing windowing) : IBindingsContext
{
    public nint GetProcAddress(string procName)
    {
        return windowing.GetProcAddress(procName);
    }
}