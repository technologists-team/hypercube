using Hypercube.Graphics.Backend.Realisation.OpenGl;
using Hypercube.Graphics.Types;

namespace Hypercube.Graphics.Backend;

public class BackendFactory
{
    public static IBackend Create(RenderBackendType type)
    {
        return type switch
        {
            RenderBackendType.OpenGL => new OpenGlBackend(),
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };
    }
}
