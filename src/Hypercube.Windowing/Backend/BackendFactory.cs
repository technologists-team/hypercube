using Hypercube.Windowing.Backend.Realisation.Glfw;
using Hypercube.Windowing.Core.Backend;
using Hypercube.Windowing.Core.Backend.Handler;
using Hypercube.Windowing.Core.Backend.Handler.Realisation;
using Hypercube.Windowing.Types;

namespace Hypercube.Windowing.Backend;

public static class BackendFactory
{
    public static BackendHandler Create(BackendProxy proxy, bool multithread)
    {
        return multithread switch
        {
            true  => new BackendHandlerMultithread(proxy),
            false => new BackendHandlerForward(proxy)
        };
    }
    
    public static IBackend Create(WindowingBackendType type)
    {
        return type switch
        {
            WindowingBackendType.Glfw => new GlfwBackend(),
            // BackendType.Sdl  => new SdlBackend(),
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };
    }
}
