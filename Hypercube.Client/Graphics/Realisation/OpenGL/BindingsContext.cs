using Hypercube.Client.Graphics.Windows;
using OpenToolkit;

namespace Hypercube.Client.Graphics.Realisation.OpenGL;

public sealed class BindingsContext(IWindowManager windowManager) : IBindingsContext
{
    public IntPtr GetProcAddress(string procName)
    {
        return windowManager.GetProcAddress(procName);
    }
}