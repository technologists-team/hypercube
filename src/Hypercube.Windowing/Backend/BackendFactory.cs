using Hypercube.Windowing.Backend.Processors;
using Hypercube.Windowing.Backend.Realisation.Glfw;

namespace Hypercube.Windowing.Backend;

public static class BackendFactory
{
    public static WindowingBackendProcessor CreateProcessor(WindowingBackendType type, bool multithread)
    {
        var backend = CreateBackend(type);
        return multithread switch
        {
            true  => new WindowingBackendProcessorMultithread(backend),
            false => new WindowingBackendProcessorForward(backend)
        };
    }

    
    public static WindowingBackendProcessor CreateProcessor(IBackend backend, bool multithread)
    {
        return multithread switch
        {
            true  => new WindowingBackendProcessorMultithread(backend),
            false => new WindowingBackendProcessorForward(backend)
        };
    }

    private static IBackend CreateBackend(WindowingBackendType type)
    {
        return type switch
        {
            WindowingBackendType.Glfw => new GlfwBackend(),
            // BackendType.Sdl  => new SdlBackend(),
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };
    }
}
