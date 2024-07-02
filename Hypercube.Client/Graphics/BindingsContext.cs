using Hypercube.Client.Graphics.Windows.Manager;
using OpenToolkit;

namespace Hypercube.Client.Graphics;

public sealed class BindingsContext(IWindowManager windowManager) : IBindingsContext
{
    public IntPtr GetProcAddress(string procName)
    {
        return windowManager.GetProcAddress(procName);
    }
}